using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SceneNetworkController : PlayerDataInstantiate {
    public GameObject PlayerPrefab;
    [SerializeField] private GameZonesController GameZonesController;
    [SerializeField] private List<GameObject> playersNickNameTablets;
    private PlayerSpawnStateController[] PlayerSpawnStateControllers;

    private float startGameDelay = 10f;
    private float totalStartGameDelay;

    private CountDownController playerCountDownController;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;

    private void Awake() {
        PlayerSpawnStateControllers = GameZonesController.ActivateGameZoneAndGetData();
        OnPlayerEnteredRoom();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    private void OnPlayerEnteredRoom() {
        PlayerSpawnStateController playerSpawnStateController = PlayerSpawnStateControllers[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, playerSpawnStateController.transform.position, Quaternion.identity);
        playerCountDownController = player.transform.Find("Main Camera").GetComponent<CountDownController>();
        ActivateCountDownPanelFunctionsAndSetTime();
        InstantiatePlayerAndGenerateData(player, playerSpawnStateController);
    }

    private void ActivateCountDownPanelFunctionsAndSetTime() {
        if(PhotonNetwork.LocalPlayer.IsMasterClient) {
            SetTimeToStartGameProperties();
            StartCoroutine(StartGameDelayCoroutine());
        } else {
            StartCoroutine(CheckUpdateTimePropertiesCoroutine());
        }
    }

    private void SetTimeToStartGameProperties() {
        Hashtable hashTable = new Hashtable();
        totalStartGameDelay = startGameDelay;
        hashTable.Add("timeToStartGame", (float)PhotonNetwork.Time + startGameDelay);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashTable);
    }

    private IEnumerator CheckUpdateTimePropertiesCoroutine() {
        while(!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("timeToStartGame")) {
            yield return null;
        }
        totalStartGameDelay = (float)PhotonNetwork.CurrentRoom.CustomProperties["timeToStartGame"] - (float)PhotonNetwork.Time;
        StartCoroutine(StartGameDelayCoroutine());
    }

    private IEnumerator StartGameDelayCoroutine() {
        while(totalStartGameDelay > 0) {
            playerCountDownController.UpdateCountDownPanel(totalStartGameDelay);
            totalStartGameDelay -= Time.unscaledDeltaTime;
            yield return null;
        };
        OnGameStarted.Invoke();
        StartCoroutine(CountDownRoundTimeCoroutine());
    }

    private IEnumerator CountDownRoundTimeCoroutine() {
        float roundTime = GetRoundTime();
        float remainingRoundTime = roundTime;
        while(remainingRoundTime > 0) {
            playerCountDownController.UpdateCountDownPanel(remainingRoundTime);
            remainingRoundTime -= Time.unscaledDeltaTime;
            yield return null;
        }
        // GEGEGEGEGEGE
    }

    private float GetRoundTime() {
        string gameDifficult = (string)PhotonNetwork.CurrentRoom.CustomProperties["Difficulty"];
        if(gameDifficult == "Easy") {
            return 20;
        } else if(gameDifficult == "Medium") {
            return 30;
        } else if(gameDifficult == "Hard") {
            return 40;
        }
        return 20;
    }
}

public class PlayerDataInstantiate : MonoBehaviourPunCallbacks {

    public void InstantiatePlayerAndGenerateData(GameObject player,PlayerSpawnStateController playerSpawnStateController) {
        Transform body = player.transform.Find("Body");
        PlayerMoveController playerMoveController = body.GetComponent<PlayerMoveController>();
        playerMoveController.OnPlayerEnteredRoom(playerSpawnStateController);
        InstantiateCameraAndSetToPlayer(player.transform, body, playerSpawnStateController, playerMoveController);
    }

    public void InstantiateCameraAndSetToPlayer(Transform player, Transform body, PlayerSpawnStateController playerSpawnStateController, PlayerMoveController playerMoveController) {
        CameraFollowingController cameraFollowingController = player.Find("Main Camera").gameObject.GetComponent<CameraFollowingController>();
        cameraFollowingController.SetPlayerAndTapController(body.transform, playerMoveController);
        cameraFollowingController.SetStartPointAndRotation(playerSpawnStateController.CameraData);
    }
}
