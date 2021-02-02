using TMPro;
using UnityEngine;

public class NicknamePanelController : MonoBehaviour
{
    public TMP_InputField inputField;

    public void OnAcceptNickname() {
        PlayerPrefs.SetString(DataStorage.NicknamePlayerPrefs, inputField.text);
        AddFunc();
    }

    public virtual void AddFunc() { }
}
