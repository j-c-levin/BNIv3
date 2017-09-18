using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_PowerupController : MonoBehaviour, IPowerupController
{
    public bool playerAlwaysHasPowerup;
    public PlayerPowerup.Powerup powerup;
    public bool didPlayerCastPowerup;
    public bool shouldDestroyPowerup;
    public powerupDuration overridePowerupDuration;
    public enum powerupDuration
    {
        Off,
        Short = 2,
        Infinite = 10000
    }
    private PlayerPowerup player;
    private int playerIdForPowerup;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowerup>();
        playerIdForPowerup = (didPlayerCastPowerup) ? 0 : 1;
        if (overridePowerupDuration != powerupDuration.Off)
        {
            player.generalPowerupDuration = (int)overridePowerupDuration;
        }
        if (playerAlwaysHasPowerup)
        {
            UsePowerup(playerIdForPowerup);
        }
    }

    public void CollectedPowerup(int playerId, Collider2D collider)
    {
        UsePowerup(playerIdForPowerup);
        if (shouldDestroyPowerup)
        {
            Destroy(collider.gameObject);
        }
    }

    public void UsePowerup(int playerId)
    {
        player.UsePowerup(playerId, powerup);
    }
}
