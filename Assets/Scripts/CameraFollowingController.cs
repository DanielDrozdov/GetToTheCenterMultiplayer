using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingController : MonoBehaviour
{
    private static CameraFollowingController Instance;

    [SerializeField] private Transform player;
    private static Vector3 playerCameraOffset = new Vector3(0,2,4);
    private float _lerpSpeed = 0.9f;
    private float _xStartRotation = 25f;

    private CameraFollowingController() { }

    private void Awake() {
        Instance = this;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + playerCameraOffset, _lerpSpeed * Time.deltaTime);
    }

    public void SetStartPointAndRotation(CameraData cameraData) {
        transform.position = cameraData.cameraStartPos;
        transform.rotation = Quaternion.Euler(_xStartRotation, cameraData.startYCameraRotation, 0);
        playerCameraOffset = transform.position - player.position;
    }

    public static CameraFollowingController GetInstance() {
        return Instance;
    }
}
