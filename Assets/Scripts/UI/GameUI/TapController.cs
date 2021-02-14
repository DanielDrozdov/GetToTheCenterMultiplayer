using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapController : MonoBehaviour, IPointerDownHandler {
    [HideInInspector] public PlayerMoveController PlayerMoveController;
    private void Start() {
        PlayerStateController.OnDisablePlayerFunctions += DeactivateTapPanel;
        enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData) {
        PlayerMoveController.Move();
    }

    private void DeactivateTapPanel() {
        gameObject.SetActive(false);
    }
}
