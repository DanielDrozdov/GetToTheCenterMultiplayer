using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LeaveRoomController : MonoBehaviourPunCallbacks
{
    private void Awake() {
        RoundResultPanelController.OnAutoDisconnect += LeaveRoom;
    }

    public void LeaveRoom() {
        if(photonView.IsMine) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2) {
            LeaveRoom();
        }
    }
}
