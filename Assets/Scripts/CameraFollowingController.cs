using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 playerCameraOffset = new Vector3(0,2,4);
    private float _lerpSpeed = 0.9f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + playerCameraOffset, _lerpSpeed * Time.deltaTime);
    }
}
