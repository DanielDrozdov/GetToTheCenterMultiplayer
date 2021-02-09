using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPanelInListController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNameText;

    public void SetData(string playerNickName,int playerNumberInRoom) {
        nickNameText.text = " " + playerNumberInRoom.ToString() + ". " + playerNickName;
    }
}
