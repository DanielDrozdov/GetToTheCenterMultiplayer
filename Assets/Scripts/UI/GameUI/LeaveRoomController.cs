using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LeaveRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject leaveRoomPanel;

    private void Awake() {
        RoundResultPanelController.OnAutoDisconnect += LeaveRoom;
    }

    public void OnButtonClick_OpenExitMenu() {
        leaveRoomPanel.SetActive(true);
    }

    public void OnButtonClick_CloseExitMenu() {
        leaveRoomPanel.SetActive(false);
    }

    public void LeaveRoom() {
        if(photonView.IsMine) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2 && PhotonNetwork.NetworkClientState != ClientState.Leaving) {
            LeaveRoom();
        }
    }
}
