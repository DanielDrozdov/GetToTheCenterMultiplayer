using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNickNameTabletController : MonoBehaviourPunCallbacks
{
    public Transform player;
    private TextMeshProUGUI nick;
    private Vector3 offSet = new Vector3(0, 0.8f, 0);
    private Camera mainCamera;

    private void Start() {
        if(!photonView.IsMine) {
            nick = GetComponent<TextMeshProUGUI>();
            nick.text = PlayerPrefs.GetString(DataStorage.NicknamePlayerPrefs);
            mainCamera = Camera.main;
        } else {
            Destroy(gameObject);
        }
    }

    private void LateUpdate() {
        nick.transform.position = player.position + offSet;
        nick.transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,mainCamera.transform.rotation * Vector3.up);
    }
}
