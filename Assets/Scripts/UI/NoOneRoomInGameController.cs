using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoOneRoomInGameController : MonoBehaviour
{
    public void OnClick_OKButton() {
        gameObject.SetActive(false);
    }
}
