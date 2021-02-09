using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class NicknameTutorialInputPanelController : NicknamePanelController
{
    public override void AddFunc() {
        DataStorage.SetPlayerNickName(inputField.text);
        transform.parent.gameObject.SetActive(false);
    }
}
