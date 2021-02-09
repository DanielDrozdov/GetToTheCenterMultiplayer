using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServersPanelController : MonoBehaviourPunCallbacks,ILobbyCallbacks
{
    [SerializeField] private Transform ServersContent;
    private RectTransform serversContentRectTransform;
    private float startingServersContentRectTransformYSizeDelta;
    private float heightRoomPanelSize = 310f;

    [SerializeField] private GameObject roomPanelPrefab;
    private static Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private Dictionary<string, GameObject> cachedRoomPanelsList = new Dictionary<string, GameObject>();

    private void Start() {
        serversContentRectTransform = ServersContent.GetComponent<RectTransform>();
        startingServersContentRectTransformYSizeDelta = serversContentRectTransform.sizeDelta.y;
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList) {
        for(int i = 0; i < roomList.Count; i++) {
            RoomInfo info = roomList[i];
            if(info.RemovedFromList) {
                cachedRoomList.Remove(info.Name);
                RemoveOldRoom(info.Name);
            } else {
                cachedRoomList[info.Name] = info;
                InstantiateNewRoom(info);
            }
        }
    }

    public static Dictionary<string, RoomInfo> GetCachedRoomDictionary() {
        return cachedRoomList;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        UpdateCachedRoomList(roomList);
        CheckContentAndSetContentSize();
    }

    private void InstantiateNewRoom(RoomInfo roomInfo) {
        if(!cachedRoomPanelsList.ContainsKey(roomInfo.Name)) {
            GameObject newRoom = Instantiate(roomPanelPrefab, ServersContent);
            newRoom.GetComponent<RoomPanelController>().SetData(roomInfo);
            cachedRoomPanelsList.Add(roomInfo.Name, newRoom);
        }
    }

    private void RemoveOldRoom(string roomName) {
        if(cachedRoomPanelsList.ContainsKey(roomName)) {
            Destroy(cachedRoomPanelsList[roomName]);
            cachedRoomPanelsList.Remove(roomName);
        }
    }

    private void CheckContentAndSetContentSize() {
        if(cachedRoomPanelsList.Values.Count >= 4) {
            serversContentRectTransform.sizeDelta = new Vector2(serversContentRectTransform.sizeDelta.x,
                startingServersContentRectTransformYSizeDelta + (cachedRoomPanelsList.Values.Count - 4) * heightRoomPanelSize);
        }
    }
}
