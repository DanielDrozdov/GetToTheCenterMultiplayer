﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameZonesController : MonoBehaviour
{
    public GameZoneController GetGameRoomGameZone() {
        GameObject gameZone = transform.Find("GameZone" + PhotonNetwork.CurrentRoom.CustomProperties["Difficulty"]).gameObject;
        gameZone.SetActive(true);
        return gameZone.GetComponent<GameZoneController>();
    }
}
