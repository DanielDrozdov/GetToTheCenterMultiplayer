using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMoveController : MonoBehaviourPunCallbacks {
    public PlayerSpawnStateController PlayerSpawnStateController;
    private Rigidbody rb;
    private float playerSpeed = 0.7f;
    private Vector3 moveDirectionVector;

    public void OnPlayerEnteredRoom(PlayerSpawnStateController playerSpawnStateController) {
        SetRigidbodyFreezeAxesPositionAndMoveDirection(playerSpawnStateController);
    }

    public void Move() {
        rb.velocity = moveDirectionVector;
    }

    private void SetRigidbodyFreezeAxesPositionAndMoveDirection(PlayerSpawnStateController playerSpawnStateController) {
        rb = GetComponent<Rigidbody>();
        if(playerSpawnStateController.GetIsBlockedZPos()) {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            moveDirectionVector = new Vector3(playerSpeed * playerSpawnStateController.GetMoveDirection().x, 0, 0);
        } else {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            moveDirectionVector = new Vector3(0, 0, playerSpeed * playerSpawnStateController.GetMoveDirection().z);
        }
    }
}
