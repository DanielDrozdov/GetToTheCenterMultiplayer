using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkMenuController : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    private void Awake() {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnJoinRandomGame() {
        if(PhotonNetwork.CountOfRooms > 0) {
            PhotonNetwork.JoinRandomRoom();
        } else {
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName + (PhotonNetwork.CountOfRooms + 1),
                new Photon.Realtime.RoomOptions() { MaxPlayers = 4 });
        }
    }

    public void OnJoinCurrentGame(string sessionName) {
        PhotonNetwork.JoinRoom(sessionName);
    }

    public void OnCreateGameSession(string sessionName) {
        PhotonNetwork.CreateRoom(sessionName, new Photon.Realtime.RoomOptions() {MaxPlayers = 4 });
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("ForestScene");

    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log(message);
    }
}
