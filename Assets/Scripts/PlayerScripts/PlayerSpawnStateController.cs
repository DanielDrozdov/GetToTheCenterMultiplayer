using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnStateController : MonoBehaviour
{
    public CameraData CameraData;
    [SerializeField] private Vector3 moveDirection;

    private void Awake() {
        CalculateCameraData();
    }

    public bool GetIsBlockedZPos() {
        if(moveDirection.z == 0) {
            return true;
        } else {
            return false;
        }
    }

    public Vector3 GetMoveDirection() {
        return moveDirection;
    }

    private void CalculateCameraData() {
        if(GetIsBlockedZPos()) {
            if(moveDirection.x < 0) {
                SetPlayerCameraData(new Vector3(4, 2, 0), 270);
            } else {
                SetPlayerCameraData(new Vector3(-4, 2, 0), 90);
            }
        } else {
            if(moveDirection.z < 0) {
                SetPlayerCameraData(new Vector3(0, 2, 4), 180);
            } else {
                SetPlayerCameraData(new Vector3(0, 2, -4), 0);
            }
        }
    }

    private void SetPlayerCameraData(Vector3 cameraOffSet,float startYCameraRotation) {
        CameraData = new CameraData(transform.position + cameraOffSet, startYCameraRotation);
    } 
}

public struct CameraData {
    public Vector3 cameraStartPos;
    public float startYCameraRotation;

    public CameraData(Vector3 _cameraStartPos, float _startYCameraRotation) {
        cameraStartPos = _cameraStartPos;
        startYCameraRotation = _startYCameraRotation;
    }
}
