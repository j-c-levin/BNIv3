using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class PowerupController : MonoBehaviour, IPowerupController
{
    private SpawnController spawnController;

    public void Start()
    {
        spawnController = GetComponent<SpawnController>();
    }

    public void CollectedPowerup(int playerId, Collider2D collider)
    {
        UsePowerup(playerId, collider.GetComponent<PowerupIcon>().powerup);
        Destroy(collider.gameObject);
    }

    public void UsePowerup(int playerId, PlayerPowerup.Powerup power)
    {
        // Iterate and let each player handle how the power up affects it
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            entry.Value.GetComponentInChildren<PlayerPowerup>().UsePowerup(playerId, power);
        }
    }
}
