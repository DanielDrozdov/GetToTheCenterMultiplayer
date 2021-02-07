using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServersPanelController : MonoBehaviourPunCallbacks,ILobbyCallbacks
{
    [SerializeField] private Transform ServersContent;
    [SerializeField] private GameObject roomPanelPrefab;
    private static Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private Dictionary<string, GameObject> cachedRoomPanelsList = new Dictionary<string, GameObject>();
    
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
    }
    
    private void InstantiateNewRoom(RoomInfo roomInfo) {
        Debug.Log("_");
        GameObject newRoom = Instantiate(roomPanelPrefab, ServersContent);
        //newRoom.transform.parent = ServersContent;
        newRoom.GetComponent<RoomPanelController>().SetData(roomInfo);
        cachedRoomPanelsList.Add(roomInfo.Name, newRoom);
    }

    private void RemoveOldRoom(string roomName) {
        Destroy(cachedRoomPanelsList[roomName]);
        cachedRoomPanelsList.Remove(roomName);
    }
}
