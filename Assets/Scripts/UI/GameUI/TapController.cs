using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TapController : MonoBehaviour, IPointerDownHandler {
    [HideInInspector] public PlayerMoveController PlayerMoveController;
    public bool IfCanMove;
    private void Start() {
        if(SceneManager.GetActiveScene().name != "TutorialScene") {
            PlayerStateController.OnDisablePlayerFunctions += DeactivateTapPanel;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(IfCanMove)
        PlayerMoveController.Move();
    }

    private void DeactivateTapPanel() {
        IfCanMove = false;
    }
}
