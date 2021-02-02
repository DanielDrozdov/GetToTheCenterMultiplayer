using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingController : MonoBehaviour
{

    private Transform player;
    [SerializeField] private TapController tapController;
    private static Vector3 playerCameraOffset = new Vector3(0,2,4);
    private float _lerpSpeed = 0.9f;
    private float _xStartRotation = 25f;

    private void Awake() {
        if(!PhotonView.Get(this).IsMine) {
            gameObject.SetActive(false);
        }
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

    public void SetPlayerAndTapController(Transform player,PlayerMoveController playerMoveController) {
        this.player = player;
        tapController.PlayerMoveController = playerMoveController;
    }

}
