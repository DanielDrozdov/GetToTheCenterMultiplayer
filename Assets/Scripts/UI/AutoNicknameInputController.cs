using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoNicknameInputController : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void OnEnable() {
        if(text == null) {
            text = GetComponent<TextMeshProUGUI>();
        }
        text.text = PlayerPrefs.GetString(DataStorage.NicknamePlayerPrefs);
    }
}
