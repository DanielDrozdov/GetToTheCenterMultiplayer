using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapController : MonoBehaviour, IPointerDownHandler {
    public void OnPointerDown(PointerEventData eventData) {
        PlayerMoveController.InstanceMove();
    }
}
