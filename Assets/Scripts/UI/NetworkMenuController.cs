using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkMenuController : MonoBehaviourPunCallbacks {
    private string gameVersion = "1";

    private void Awake() {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomName,string mapName) {
        Hashtable properties = new Hashtable();
        properties.Add("MapName", mapName);
        PhotonNetwork.CreateRoom(roomName,
                new Photon.Realtime.RoomOptions() { MaxPlayers = 4 ,CustomRoomProperties = properties});
    }

    public void OnJoinRandomGame() {
        if(PhotonNetwork.CountOfRooms > 0) {
            PhotonNetwork.JoinRandomRoom();
        } else {
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName + (PhotonNetwork.CountOfRooms + 1),
                new Photon.Realtime.RoomOptions() { MaxPlayers = 4 });
        }
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log(message);
    }
}
