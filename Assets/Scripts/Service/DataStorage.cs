using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class DataStorage
{
    public readonly static string IsFirstEnterInGamePlayerPrefs = "IsFirstEnterInGame";
    public readonly static string NicknamePlayerPrefs = "Nickname";

    public static void SetPlayerNickName(string newName) {
        PlayerPrefs.SetString(NicknamePlayerPrefs, newName);
        SetPlayerNickNameToPhotonNetwork();
    }

    public static void SetPlayerNickNameToPhotonNetwork() {
        if(PlayerPrefs.HasKey(NicknamePlayerPrefs)) {
            PhotonNetwork.NickName = PlayerPrefs.GetString(NicknamePlayerPrefs);
        }
    }
}
