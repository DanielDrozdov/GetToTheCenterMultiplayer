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
    public delegate void PlayerActionsToAllPlayers(WinPlayerAnimationData animData);
    public static event PlayerActions OnDisablePlayerFunctions;
    public static event PlayerActionsToAllPlayers OnPlayerWinToAllPlayersEvent;

    private string winPlayerNickName;
    private float winPlayersCount;

    private void Awake() {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkEventReceived;
        SceneNetworkController.OnGameEnd += SetPlayerInactive;
    }

    public void OnPlayerWinGame() {
        if(photonView.IsMine) {
            OnDisablePlayerFunctions();
            SetWinPlayerDataToAllPlayers();
            WinPlayerAnimationData animData = new WinPlayerAnimationData(winPlayerNickName, winPlayersCount);
            OnPlayerWinToAllPlayersEvent(animData);
        }
    }

    private void SetPlayerInactive() {
        OnDisablePlayerFunctions();
    }

    private void NetworkEventReceived(EventData obj) {
        if(obj.Code == 0) {
            object[] data = (object[])obj.CustomData;
            winPlayerNickName = (string)data[0];
            winPlayersCount = (float)data[1];
            WinPlayerAnimationData animData = new WinPlayerAnimationData(winPlayerNickName, winPlayersCount);
            OnPlayerWinToAllPlayersEvent(animData);
        }
    }

    private void SetWinPlayerDataToAllPlayers() {
        winPlayerNickName = PhotonNetwork.NickName;
        winPlayersCount++;
        object[] data = { winPlayerNickName, winPlayersCount};
        PhotonNetwork.RaiseEvent(0, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
}

public class WinPlayerAnimationData {
    public string winnerName;
    public float winPlayersCount;

    public WinPlayerAnimationData(string _winnerName,float _winPlayersCount) {
        winnerName = _winnerName;
        winPlayersCount = _winPlayersCount;
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

