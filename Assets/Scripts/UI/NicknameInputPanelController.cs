using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameInputPanelController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void OnAcceptNickname() {
        PlayerPrefs.SetString(DataStorage.NicknamePlayerPrefs, inputField.text);
        transform.parent.gameObject.SetActive(false);
    }
}
