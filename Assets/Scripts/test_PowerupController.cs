using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_PowerupController : MonoBehaviour, IPowerupController
{
    public PlayerPowerup.Powerup powerup;
    public bool didPlayerCastPowerup;
    public PlayerPowerup player;
    public bool shouldDestroyPowerup;
    bool playerHasPowerup = false;

    public void CollectedPowerup(int playerId, Collider2D powerup)
    {
        playerHasPowerup = true;
        if (shouldDestroyPowerup)
        {
            Destroy(powerup.gameObject);
        }
    }

    public bool HasPowerup(int playerId)
    {
        return playerHasPowerup;
    }

    public void UsePowerup(int playerId)
    {
        playerId = (didPlayerCastPowerup) ? 0 : 1;
        player.UsePowerup(playerId, powerup);
        playerHasPowerup = false;
    }
}
