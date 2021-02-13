using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerCanvasNetworkController : MonoBehaviour
{
    [HideInInspector] public PlayerCanvasController PlayerCanvasController;
    private SceneNetworkController SceneNetworkController;
    private Queue<WinPlayerAnimationData> winAnimationsQueue = new Queue<WinPlayerAnimationData>(4);
    private static bool IsWinAnimEnd;
    private bool IsCheckNextCoroutineStarted;

    private float startGameDelay = 10f;
    private float totalStartGameDelay;

    private void Awake() {
        SceneNetworkController = GetComponent<SceneNetworkController>();
    }

    public static void OnWinnerRatingEnd() {
        IsWinAnimEnd = true;
    }

    public void PlayWinTextAnimationForAllPlayers(WinPlayerAnimationData winPlayerAnimationData) {
        winAnimationsQueue.Enqueue(winPlayerAnimationData);
        if(!IsCheckNextCoroutineStarted) {
            StartCoroutine(CheckNextWinAnim());
        }
    }

    public void ActivateCountDownPanelFunctionsAndSetTime() {
        if(PhotonNetwork.LocalPlayer.IsMasterClient) {
            totalStartGameDelay = startGameDelay;
            SetTimeToStartGameProperties();
            StartCoroutine(StartGameDelayCoroutine());
        } else {
            StartCoroutine(CheckUpdateTimePropertiesCoroutine());
        }
    }

    private void SetTimeToStartGameProperties() {
        Hashtable hashTable = new Hashtable();
        hashTable.Add("timeToStartGame", (float)PhotonNetwork.Time + startGameDelay);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashTable);
    }

    private IEnumerator CheckUpdateTimePropertiesCoroutine() {
        while(!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("timeToStartGame")) {
            yield return null;
        }
        totalStartGameDelay = (float)PhotonNetwork.CurrentRoom.CustomProperties["timeToStartGame"] - (float)PhotonNetwork.Time;
        StartCoroutine(StartGameDelayCoroutine());
    }

    private IEnumerator StartGameDelayCoroutine() {
        while(totalStartGameDelay > 0) {
            PlayerCanvasController.UpdateCountDownPanel(totalStartGameDelay);
            totalStartGameDelay -= Time.unscaledDeltaTime;
            yield return null;
        };
        SceneNetworkController.OnGameStarted_CallEvent();
        PlayerCanvasController.PlayStartTextAnimation();
        StartCoroutine(CountDownRoundTimeCoroutine());
    }

    private IEnumerator CountDownRoundTimeCoroutine() {
        float roundTime = GetRoundTime();
        float remainingRoundTime = roundTime;
        while(remainingRoundTime > 0) {
            PlayerCanvasController.UpdateCountDownPanel(remainingRoundTime);
            remainingRoundTime -= Time.unscaledDeltaTime;
            yield return null;
        }
        SceneNetworkController.OnGameEnd_CallEvent();
    }

    private float GetRoundTime() {
        string gameDifficult = (string)PhotonNetwork.CurrentRoom.CustomProperties["Difficulty"];
        if(gameDifficult == "Easy") {
            return 20;
        } else if(gameDifficult == "Medium") {
            return 30;
        } else if(gameDifficult == "Hard") {
            return 40;
        }
        return 20;
    }

    private IEnumerator CheckNextWinAnim() {
        IsCheckNextCoroutineStarted = true;
        while(winAnimationsQueue.Count != 0) {
            WinPlayerAnimationData animData = winAnimationsQueue.Dequeue();
            PlayerCanvasController.PlayPlayerWinRatingTextAnimation(animData.GetTextAboutWinner());
            while(!IsWinAnimEnd) {
                yield return null;
            }
            IsWinAnimEnd = false;
        }
        IsCheckNextCoroutineStarted = false;
    }
}
