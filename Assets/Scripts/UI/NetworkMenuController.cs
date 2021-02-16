﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using UnityEngine.Networking;

public class NetworkMenuController : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject NoOneRoomInGamePanel;
    [SerializeField] private GameObject NoInternetConnection;

    private string gameVersion = "1";
    private string[] maps = { "Forest", "Desert" };

    private void Awake() {
        StartCoroutine(checkInternetConnection((isConnected) => {
            if(isConnected) {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.AutomaticallySyncScene = true;
                DataStorage.SetPlayerNickNameToPhotonNetwork();
                PhotonNetwork.ConnectUsingSettings();
            } else {
                NoInternetConnection.SetActive(true);
            }
        }));
    }

    public void CreateRoom(string roomName,string mapName,string difficulty) {
        Hashtable properties = new Hashtable();
        properties.Add("MapName", mapName);
        properties.Add("Difficulty", difficulty);
        string[] lobbyProperties = { "MapName"};
        PhotonNetwork.CreateRoom(roomName,
                new RoomOptions() { MaxPlayers = 4 ,CustomRoomProperties = properties,CustomRoomPropertiesForLobby = lobbyProperties });
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

    IEnumerator checkInternetConnection(Action<bool> action) {
        WWW www = new WWW("http://google.com");
        yield return www;
        if(www.error != null) {
            action(false);
        } else {
            action(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log(message);
    }
}
