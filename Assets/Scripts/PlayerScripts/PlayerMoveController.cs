using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour {
    public PlayerSpawnStateController PlayerSpawnStateController;
    private static PlayerMoveController Instance;
    private Rigidbody rb;
    private float playerSpeed = 0.8f;
    private Vector3 moveDirectionVector;

    private PlayerMoveController() { }

    private void Awake() {
        Instance = this;
        transform.position = PlayerSpawnStateController.transform.position;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        SetRigidbodyFreezeAxesPositionAndMoveDirection();
        CameraFollowingController.GetInstance().SetStartPointAndRotation(PlayerSpawnStateController.CameraData);
    }

    public static void InstanceMove() {
        Instance.Move();
    }

    private void Move() {
        rb.velocity = moveDirectionVector;
    }

    private void SetRigidbodyFreezeAxesPositionAndMoveDirection() {
        if(PlayerSpawnStateController.GetIsBlockedZPos()) {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            moveDirectionVector = new Vector3(playerSpeed * PlayerSpawnStateController.GetMoveDirection().x, 0, 0);
        } else {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            moveDirectionVector = new Vector3(0, 0, playerSpeed * PlayerSpawnStateController.GetMoveDirection().z);
        }
    }
}
