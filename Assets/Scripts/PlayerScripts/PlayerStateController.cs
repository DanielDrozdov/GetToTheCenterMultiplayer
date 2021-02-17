using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class PlayerStateController : MonoBehaviourPun
{
    [SerializeField] private PlayerCanvasController playerCanvasController;

    public delegate void PlayerActions();
    public delegate void PlayerActionsToAllPlayers(WinPlayerData animData);
    public static event PlayerActions OnDisablePlayerFunctions;
    public static event PlayerActionsToAllPlayers OnPlayerWinToAllPlayersEvent;

    private bool IsPlayerWin;

    private string winPlayerNickName;
    private float winPlayersCount;
    private float timeScore;

    private void Awake() {
            SceneNetworkController.OnGameEnd += SetPlayerInactive;
    }

    private void OnEnable() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkEventReceived;
    }

    private void OnDisable() {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkEventReceived;
    }

    public void OnPlayerWinGame() {
        if(photonView.IsMine) {
            OnDisablePlayerFunctions();
            IsPlayerWin = true;
            SetPlayerDataToAllPlayers();
        }
    }

    private void SetPlayerInactive() {
        OnDisablePlayerFunctions();
        if(photonView.IsMine) {
            if(!IsPlayerWin) {
                SetPlayerDataToAllPlayers();
            }
        }
    }

    private void NetworkEventReceived(EventData obj) {
        if(obj.Code == 0) {
            object[] data = (object[])obj.CustomData;
            string _winPlayerNickName = (string)data[0];
            float _winPlayersCount = (float)data[1];
            winPlayersCount = _winPlayersCount;
            float _timeScore = (float)data[2];
            bool _IsPlayerWin = (bool)data[3];
            if(photonView.IsMine) {
                WinPlayerData winnerData = new WinPlayerData(_winPlayerNickName, winPlayersCount, _timeScore, _IsPlayerWin);
                OnPlayerWinToAllPlayersEvent(winnerData);
                if(_IsPlayerWin) {
                    AudioSceneController.GetInstance().PlayWinAudio();
                }
            }
        }
    }

    private object[] GetPlayerData() {
        winPlayerNickName = PhotonNetwork.NickName;
        winPlayersCount++;
        timeScore = (float)PhotonNetwork.Time - (float)PhotonNetwork.CurrentRoom.CustomProperties["timeToStartGame"];
        if(!IsPlayerWin) {
            timeScore = 20f;
        }
        object[] data = { winPlayerNickName, winPlayersCount, timeScore, IsPlayerWin };
        return data;
    }

    private void SetPlayerDataToAllPlayers() {
        PhotonNetwork.RaiseEvent(0, GetPlayerData(), RaiseEventOptions.Default, SendOptions.SendUnreliable);
        WinPlayerData winnerData = new WinPlayerData(winPlayerNickName, winPlayersCount, timeScore, IsPlayerWin);
        OnPlayerWinToAllPlayersEvent(winnerData);
    }
}

public class WinPlayerData {
    public string winnerName;
    public float winPlayersCount;
    public float timeScore;
    public bool IsPlayerWin;

    public WinPlayerData(string _winnerName,float _winPlayersCount,float _timeScore,bool _IsPlayerWin) {
        winnerName = _winnerName;
        winPlayersCount = _winPlayersCount;
        timeScore = _timeScore;
        IsPlayerWin = _IsPlayerWin;
    }

    public string GetTextAboutWinner() {
        string text = "";
        if(winPlayersCount == 1) {
            text = "1-st " + winnerName;
            return text;
        } else if(winPlayersCount == 2) {
            text = "2-nd " + winnerName;
            return text;
        } else if(winPlayersCount == 3) {
            text = "3-rd " + winnerName;
            return text;
        } else if(winPlayersCount == 4) {
            text = "4-th " + winnerName;
            return text;
        }
        return text;
    }
}

