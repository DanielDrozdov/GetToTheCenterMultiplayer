using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SceneNetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerSpawnStateController[] PlayerSpawnStateControllers;
    public GameObject PlayerPrefab;
    public GameObject PlayerCamera;
    
    private void Awake() {
        OnPlayerEnteredRoom();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    private void OnPlayerEnteredRoom() {
        PlayerSpawnStateController playerSpawnStateController = PlayerSpawnStateControllers[PhotonNetwork.PlayerList.Length - 1];
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, playerSpawnStateController.transform.position, Quaternion.identity);
        PlayerMoveController playerMoveController = player.GetComponent<PlayerMoveController>();
        playerMoveController.OnPlayerEnteredRoom(playerSpawnStateController);
        InstantiateCameraAndSetToPlayer(player.transform, playerSpawnStateController, playerMoveController);
    }

    private void InstantiateCameraAndSetToPlayer(Transform player,PlayerSpawnStateController playerSpawnStateController,PlayerMoveController playerMoveController) {
        GameObject camera = PhotonNetwork.Instantiate(PlayerCamera.name, player.position, Quaternion.identity);
        CameraFollowingController cameraFollowingController = camera.GetComponent<CameraFollowingController>();
        cameraFollowingController.SetPlayerAndTapController(player.transform, playerMoveController);
        cameraFollowingController.SetStartPointAndRotation(playerSpawnStateController.CameraData);
    }
}
