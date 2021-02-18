using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TapController : MonoBehaviour, IPointerDownHandler {
    [HideInInspector] public PlayerMoveController PlayerMoveController;
    private void Start() {
        if(SceneManager.GetActiveScene().name != "TutorialScene") {
            PlayerStateController.OnDisablePlayerFunctions += DeactivateTapPanel;
            enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        PlayerMoveController.Move();
    }

    private void DeactivateTapPanel() {
        gameObject.SetActive(false);
    }
}
