using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_PowerupController : MonoBehaviour, IPowerupController
{
	public PlayerPowerup player;
    bool playerHasPowerup = false;
    public void CollectedPowerup(int playerId, Collider2D powerup)
    {
        playerHasPowerup = true;
    }

    public bool HasPowerup(int playerId)
    {
        return playerHasPowerup;
    }

    public void UsePowerup(int playerId)
    {
		player.JupiterJump(0);
        playerHasPowerup = false;
    }
}
