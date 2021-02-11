using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapController : MonoBehaviour, IPointerDownHandler {
    [HideInInspector] public PlayerMoveController PlayerMoveController;
    private void Start() {
        SceneNetworkController.OnGameStarted += ActivateTapController;
        enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        PlayerMoveController.Move();
    }

    private void ActivateTapController() {
        enabled = true;
    }
}
