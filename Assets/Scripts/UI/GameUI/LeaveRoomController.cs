using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LeaveRoomController : MonoBehaviourPunCallbacks
{
    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2) {
            LeaveRoom();
        }
    }
}
