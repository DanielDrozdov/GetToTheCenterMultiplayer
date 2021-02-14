using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SceneNetworkController : PlayerDataInstantiate {
    public GameObject PlayerPrefab;
    private static Transform cameraMainPoint;
    private PlayerSpawnStateController[] PlayerSpawnStateControllers;
    private PlayerCanvasNetworkController playerCanvasNetworkController;
    [SerializeField] private GameZonesController GameZonesController;
    [SerializeField] private List<GameObject> playersNickNameTablets;

    private PlayerCanvasController playerCanvasController;

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameEnd;

    private void Awake() {
        GameZoneController roomGameZoneController = GameZonesController.GetGameRoomGameZone();
        cameraMainPoint = roomGameZoneController.CameraMainPoint;
        PlayerSpawnStateControllers = roomGameZoneController.PlayerSpawnStateControllers;
        playerCanvasNetworkController = GetComponent<PlayerCanvasNetworkController>();
        OnPlayerEnteredRoom();
    }

    public static Transform GetCameraMainPoint() {
        return cameraMainPoint;
    }

    public void OnGameStarted_CallEvent() {
        OnGameStarted();
    }

    public void OnGameEnd_CallEvent() {
        OnGameEnd();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    private void OnPlayerEnteredRoom() {
        PlayerSpawnStateController playerSpawnStateController = PlayerSpawnStateControllers[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, playerSpawnStateController.transform.position, Quaternion.identity);
        GameObject playerCamera = player.transform.Find("Main Camera").gameObject;
        playerCanvasController = playerCamera.GetComponent<PlayerCanvasController>();
        playerCanvasNetworkController.PlayerCanvasController = playerCanvasController;
        PlayerStateController.OnPlayerWinToAllPlayersEvent += playerCanvasNetworkController.SendNewWinnerData;
        playerCanvasNetworkController.ActivateCountDownPanelFunctionsAndSetTime();
        InstantiatePlayerAndGenerateData(player, playerSpawnStateController, playerCamera);
    }

}

public class PlayerDataInstantiate : MonoBehaviourPunCallbacks {

    public void InstantiatePlayerAndGenerateData(GameObject player,PlayerSpawnStateController playerSpawnStateController,GameObject playerCamera) {
        Transform body = player.transform.Find("Body");
        PlayerMoveController playerMoveController = body.GetComponent<PlayerMoveController>();
        playerMoveController.OnPlayerEnteredRoom(playerSpawnStateController,playerCamera);
        InstantiateCameraAndSetToPlayer(player.transform, body, playerSpawnStateController, playerMoveController);
    }

    public void InstantiateCameraAndSetToPlayer(Transform player, Transform body, PlayerSpawnStateController playerSpawnStateController, PlayerMoveController playerMoveController) {
        CameraFollowingController cameraFollowingController = player.Find("Main Camera").gameObject.GetComponent<CameraFollowingController>();
        cameraFollowingController.SetPlayerAndTapController(body.transform, playerMoveController);
        cameraFollowingController.SetStartPointAndRotation(playerSpawnStateController.CameraData);
    }
}
