using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorialInstantiate : PlayerDataInstantiate
{
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerSpawnStateController playerSpawnStateController;

    void Start()
    {
        InstantiatePlayerAndGenerateData(player, playerSpawnStateController);
    }

    
}
