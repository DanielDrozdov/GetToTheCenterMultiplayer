using TMPro;
using UnityEngine;

public class NicknamePanelController : MonoBehaviour
{
    public TMP_InputField inputField;

    public void OnAcceptNickname() {
        DataStorage.SetPlayerNickName(inputField.text);
        AddFunc();
    }

    public virtual void AddFunc() { }
}
