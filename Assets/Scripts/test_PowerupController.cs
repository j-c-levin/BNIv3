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
    private bool playerHasPowerup = false;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowerup>();
        if (overridePowerupDuration != powerupDuration.Off)
        {
            player.generalPowerupDuration = (int)overridePowerupDuration;
        }
    }

    public PlayerPowerup.Powerup CollectedPowerup(int playerId, Collider2D powerup)
    {
        playerHasPowerup = true;
        if (shouldDestroyPowerup)
        {
            Destroy(powerup.gameObject);
        }
        return this.powerup;
    }

    public bool HasPowerup(int playerId)
    {
        return playerHasPowerup || playerAlwaysHasPowerup;
    }

    public void UsePowerup(int playerId)
    {
        playerId = (didPlayerCastPowerup) ? 0 : 1;
        player.UsePowerup(playerId, powerup);
        playerHasPowerup = false;
    }
}
