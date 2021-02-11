using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkMenuController : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject NoOneRoomInGamePanel;

    private string gameVersion = "1";
    private string[] maps = { "Forest", "Desert" };

    private void Awake() {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        DataStorage.SetPlayerNickNameToPhotonNetwork();
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomName,string mapName,string difficulty) {
        Hashtable properties = new Hashtable();
        properties.Add("MapName", mapName);
        properties.Add("Difficulty", difficulty);
        PhotonNetwork.CreateRoom(roomName,
                new RoomOptions() { MaxPlayers = 4 ,CustomRoomProperties = properties});
    }

    public void OnJoinRandomGame() {
        if(PhotonNetwork.CountOfRooms > 0) {
            PhotonNetwork.JoinRandomRoom();
            EnableOrDisableMenuPanels EnableDisablePanelsController = EnableOrDisableMenuPanels.GetInstance();
            EnableDisablePanelsController.DeactivateMainPanelsForLobby();
            EnableDisablePanelsController.ActivateLobby();
        } else {
            NoOneRoomInGamePanel.SetActive(true);
        }
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log(message);
    }
}
