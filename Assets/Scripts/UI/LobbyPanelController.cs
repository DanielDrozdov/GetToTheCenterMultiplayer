using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyPanelController : MonoBehaviourPunCallbacks {
    [SerializeField] private TextMeshProUGUI mapNameText;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI difficultyText;

    [SerializeField] private GameObject playerPanelPrefab;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private Transform playerList;
    private Dictionary<int, GameObject> playersPanels = new Dictionary<int, GameObject>();

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        AddPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        RemovePlayer(otherPlayer);
    }

    public override void OnJoinedRoom() {
        UpdatePlayerListAndCheckPlayerNumberInRoom();
        UpdateLobbyUI();
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1) {
            ActivateHostPlayButton();
        }
    }

    public override void OnLeftRoom() {
        EnableOrDisableMenuPanels.GetInstance().ActivateMenu();
        ClearPlayerListAndDeletePlayersPanels();
        gameObject.SetActive(false);
    }

    public void Disconnect() {
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame() {
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2) {
            return;
        } 
        int sceneNumberInBuild = SceneManager.GetSceneByName((string)PhotonNetwork.CurrentRoom.CustomProperties["MapName"] + "Scene").buildIndex;
        PhotonNetwork.LoadLevel(2);
    }

    private void AddPlayer(Player player) {
        GameObject newPlayerPanel = Instantiate(playerPanelPrefab, playerList);
        newPlayerPanel.GetComponent<PlayerPanelInListController>().SetData(player.NickName, playersPanels.Count + 1);
        playersPanels.Add(player.ActorNumber, newPlayerPanel);
    }

    private void RemovePlayer(Player player) {
        Destroy(playersPanels[player.ActorNumber]);
        playersPanels.Remove(player.ActorNumber);
    }

    private void UpdatePlayerListAndCheckPlayerNumberInRoom() {
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values) {
            AddPlayer(player);
        }
    }

    private void UpdateLobbyUI() {
        mapNameText.text = "Map name: " + PhotonNetwork.CurrentRoom.CustomProperties["MapName"];
        difficultyText.text = "Difficulty: " + PhotonNetwork.CurrentRoom.CustomProperties["Difficulty"];
        roomNameText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
    }

    private void ActivateHostPlayButton() {
        startGameButton.SetActive(true);
    }

    private void ClearPlayerListAndDeletePlayersPanels() {
        foreach(GameObject playerPanel in playersPanels.Values) {
            Destroy(playerPanel);
        }
        playersPanels.Clear();
    }

}
