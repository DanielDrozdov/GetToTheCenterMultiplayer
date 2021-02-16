using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI mapName;
    [SerializeField] private TextMeshProUGUI playerInRoomCount;
    private RoomInfo roomInfo;

    public void SetData(RoomInfo roomInfo) {
        roomName.text = "Room name: " + roomInfo.Name;
        mapName.text = "Map: " + (string)roomInfo.CustomProperties["MapName"];
        this.roomInfo = roomInfo;
        UpdatePlayersCount();
    }

    public void OnClick_ConnectButton() {
        if(roomInfo.PlayerCount == roomInfo.MaxPlayers) {
            return;
        }
        EnableOrDisableMenuPanels EnableDisablePanelsController = EnableOrDisableMenuPanels.GetInstance();
        PhotonNetwork.JoinRoom(roomInfo.Name);
        EnableDisablePanelsController.DeactivateMainPanelsForLobby();
    }

    private void OnEnable() {
        UpdatePlayersCount();
    }

    private void UpdatePlayersCount() {
        if(roomInfo == null) return;
        playerInRoomCount.text = roomInfo.PlayerCount.ToString() + "/4";
    }
}
