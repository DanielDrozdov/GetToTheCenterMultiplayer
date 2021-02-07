using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SceneNetworkController : PlayerDataInstantiate
{
    [SerializeField] private PlayerSpawnStateController[] PlayerSpawnStateControllers;
    [SerializeField] private List<GameObject> playersNickNameTablets;
    public GameObject PlayerPrefab;
    
    private void Awake() {
        OnPlayerEnteredRoom();
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    private void OnPlayerEnteredRoom() {
        PlayerSpawnStateController playerSpawnStateController = PlayerSpawnStateControllers[PhotonNetwork.PlayerList.Length - 1];
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefab.name, playerSpawnStateController.transform.position, Quaternion.identity);
        InstantiatePlayerAndGenerateData(player, playerSpawnStateController);
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
