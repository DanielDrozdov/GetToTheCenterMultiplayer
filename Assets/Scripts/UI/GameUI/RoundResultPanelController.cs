using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class RoundResultPanelController : MonoBehaviourPunCallbacks {
    [SerializeField] private TextMeshProUGUI namesPanel;
    [SerializeField] private TextMeshProUGUI scoreResultsPanel;
    [SerializeField] private TextMeshProUGUI autoDisconnectText;
    [SerializeField] private Button quitButton;
    private float autoDisconnectTime = 15f;
    private float remainingDisconnectTime;

    private Queue<WinPlayerData> winPlayerDatas;

    public delegate void RoundResultActions();
    public static event RoundResultActions OnAutoDisconnect;

    private void Awake() {
        StartCoroutine(OneSecondActivateDelay());    
    }

    public void OffAnimation() {
        GetComponent<Animator>().enabled = false;
    }

    public void OnClickButton_Quit() {
        OnAutoDisconnect();
    }

    private void OnUpdateScoreResultsAndNames() {
        string resultText = "Time result:\n";
        string namesText = "Player name:\n";
        float winPlayersCount = winPlayerDatas.Count;
        for(int i = 1; i <= winPlayersCount; i++) {
            WinPlayerData playerData = winPlayerDatas.Dequeue();
            resultText += string.Format("{0:f}", playerData.timeScore) + " seconds\n";
            namesText += i.ToString() + ". " + playerData.winnerName + "\n";
        }
        scoreResultsPanel.text = resultText;
        namesPanel.text = namesText;
    }

    private IEnumerator OneSecondActivateDelay() {
        AudioSceneController.GetInstance().PlayGameEndAudio();
        yield return new WaitForSeconds(1);
        winPlayerDatas = PlayerCanvasNetworkController.GetWinPlayersData();
        StartCoroutine(AutoDisconnectCoroutine());
        OnUpdateScoreResultsAndNames();
    }

    private IEnumerator AutoDisconnectCoroutine() {
        remainingDisconnectTime = autoDisconnectTime;
        int oldInt = 0;
        while(remainingDisconnectTime > 0) {
            remainingDisconnectTime -= Time.deltaTime;
            float newInteger = Mathf.Floor(remainingDisconnectTime);
            if(oldInt != newInteger && newInteger >= 0) {
                autoDisconnectText.text = newInteger.ToString();
                oldInt = (int)newInteger;
            }
            yield return null;
        }
        OnAutoDisconnect();
    }
}
