using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorialInstantiate : PlayerDataInstantiate
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerSpawnStateController playerSpawnStateController;
    [SerializeField] private GameObject playerCamera;

    void Start()
    {
        InstantiatePlayerAndGenerateData(player, playerSpawnStateController, playerCamera);
    }

    
}
