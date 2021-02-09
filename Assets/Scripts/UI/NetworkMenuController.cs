using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkMenuController : MonoBehaviourPunCallbacks {
    [SerializeField] private TextMeshProUGUI timeToNextRandomGameText;
    private bool IsRandomPlayBlocked;

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
        if(!IsRandomPlayBlocked) {
            if(PhotonNetwork.CountOfRooms > 0) {
                bool isNotConnected = PhotonNetwork.JoinRandomRoom();
                if(isNotConnected) {
                    StartCoroutine(FiveSecondsDelayToRandomPlay());
                    return;
                }
            } else {
                string roomName = PhotonNetwork.NickName + (PhotonNetwork.CountOfRooms + 1);
                CreateRoom(roomName, maps[Random.Range(0, maps.Length)], "Medium");
            }
            EnableOrDisableMenuPanels EnableDisablePanelsController = EnableOrDisableMenuPanels.GetInstance();
            EnableDisablePanelsController.DeactivateMainPanelsForLobby();
            EnableDisablePanelsController.ActivateLobby();
        }
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message) {
        Debug.Log(message);
    }

    private IEnumerator FiveSecondsDelayToRandomPlay() {
        float leftTime = 5f;
        IsRandomPlayBlocked = true;
        timeToNextRandomGameText.gameObject.SetActive(true);
        while(leftTime >= 0) {
            leftTime -= Time.deltaTime;
            timeToNextRandomGameText.text = Mathf.Floor(leftTime).ToString();
            yield return null;
        }
        IsRandomPlayBlocked = false;
        timeToNextRandomGameText.gameObject.SetActive(false);
    }
}
