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
        this.roomInfo = roomInfo;
        UpdatePlayersCount();
    }

    private void OnEnable() {
        UpdatePlayersCount();
    }

    private void UpdatePlayersCount() {
        if(roomInfo == null) return;
        playerInRoomCount.text = roomInfo.PlayerCount.ToString() + "/4";
    }
}
