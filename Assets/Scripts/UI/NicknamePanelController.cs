using TMPro;
using UnityEngine;

public class NicknamePanelController : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject wrongNickNameText;

    public void OnAcceptNickname() {
        bool isNormalNickName = CheckForNotLatin(inputField.text);
        if(isNormalNickName) {
            DataStorage.SetPlayerNickName(inputField.text);
            AddFunc();
        } else {
            wrongNickNameText.SetActive(true);
        }
    }

    public virtual void AddFunc() { }

    bool CheckForNotLatin(string stringToCheck) {
        bool boolToReturn = false;
        foreach(char c in stringToCheck) {
            int code = c;
            if((code > 96 && code < 123) || (code > 64 && code < 91))
                boolToReturn = true;
        }
        return boolToReturn;
    }

    private void OnEnable() {
        inputField.text = PlayerPrefs.GetString(DataStorage.NicknamePlayerPrefs);
    }

    private void OnDisable() {
        wrongNickNameText.SetActive(false);
    }
}
