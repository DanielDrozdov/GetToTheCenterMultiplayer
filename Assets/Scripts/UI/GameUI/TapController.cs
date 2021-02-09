using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapController : MonoBehaviour, IPointerDownHandler {
    [HideInInspector] public PlayerMoveController PlayerMoveController;
    public void OnPointerDown(PointerEventData eventData) {
        PlayerMoveController.Move();
    }
}
