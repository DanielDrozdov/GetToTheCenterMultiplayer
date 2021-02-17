using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class PlayerCanvasNetworkController : MonoBehaviourPunCallbacks
{
    [HideInInspector] public PlayerCanvasController PlayerCanvasController;
    private SceneNetworkController SceneNetworkController;
    private Queue<WinPlayerData> winPlayersDataQueueDynamic = new Queue<WinPlayerData>(4);
    private static Queue<WinPlayerData> winPlayersDataQueue = new Queue<WinPlayerData>(4);
    private bool IsGameEnd;
    private static bool IsWinAnimEnd;
    private bool IsCheckNextCoroutineStarted;

    private float startGameDelay = 10f;
    private float totalStartGameDelay;

    private bool isGameLoaded;

    private void Awake() {
        SceneNetworkController = GetComponent<SceneNetworkController>();
    }

    public static void OnWinnerRatingEnd() {
        IsWinAnimEnd = true;
    }

    public static Queue<WinPlayerData> GetWinPlayersData() {
        return winPlayersDataQueue;
    }

    public void SendNewWinnerData(WinPlayerData winPlayerData) {
        if(winPlayerData.IsPlayerWin) {
            winPlayersDataQueueDynamic.Enqueue(winPlayerData);
        }
        winPlayersDataQueue.Enqueue(winPlayerData);
        if(!IsCheckNextCoroutineStarted) {
            StartCoroutine(CheckNextWinAnim());
        }

        if(winPlayerData.IsPlayerWin == true && winPlayerData.winPlayersCount == PhotonNetwork.CurrentRoom.PlayerCount) {
            OnEndGame();
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
        AudioSceneController audioSceneController = AudioSceneController.GetInstance();
        while(totalStartGameDelay > 0) {
            bool isPanelUpdated = PlayerCanvasController.UpdateCountDownPanel(totalStartGameDelay);
            if(isPanelUpdated && photonView.IsMine) {
                audioSceneController.PlayTickAudio();
            }
            totalStartGameDelay -= Time.unscaledDeltaTime;
            yield return null;
        };
        if(photonView.IsMine) {
            audioSceneController.PlayGongAudio();
        }
        UpdateGameStatedBool();
        SceneNetworkController.OnGameStarted_CallEvent();
        isGameLoaded = true;
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
        OnEndGame();
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
        while(winPlayersDataQueueDynamic.Count != 0) {
            WinPlayerData playerData = winPlayersDataQueueDynamic.Dequeue();
            PlayerCanvasController.PlayPlayerWinRatingTextAnimation(playerData.GetTextAboutWinner());
            while(!IsWinAnimEnd) {
                yield return null;
            }
            IsWinAnimEnd = false;
        }
        IsCheckNextCoroutineStarted = false;
    }

    private void OnEndGame() {
        if(!IsGameEnd) {
            SceneNetworkController.OnGameEnd_CallEvent();
            IsGameEnd = true;
        }
    }

    private IEnumerator VariablesCheckCoroutine() {
        while(!isGameLoaded) {
            yield return null;
        }
        
    }

    [PunRPC]
    public void UpdateGameStatedBool() {
        isGameLoaded = true;
        Debug.Log("fdf");
    }
}
