using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TapController : MonoBehaviour, IPointerDownHandler {
    [HideInInspector] public PlayerMoveController PlayerMoveController;
    private AudioSceneController audioSceneController;
    private void Start() {
        if(SceneManager.GetActiveScene().name != "TutorialScene") {
            PlayerStateController.OnDisablePlayerFunctions += DeactivateTapPanel;
            enabled = false;
        }
        audioSceneController = AudioSceneController.GetInstance();
    }

    public void OnPointerDown(PointerEventData eventData) {
        PlayerMoveController.Move();
        audioSceneController.PlayTapAudio();
    }

    private void DeactivateTapPanel() {
        gameObject.SetActive(false);
    }
}
