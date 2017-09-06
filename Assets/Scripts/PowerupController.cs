using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour, IPowerupController
{
    private enum Powerup
    {
        None,
        JupiterJump
    }
    private Dictionary<int, PlayerPowerup> playersDictionary;
    private Dictionary<int, Powerup> hasPowerupDictionary;

    public void Start()
    {
        playersDictionary = new Dictionary<int, PlayerPowerup>();
        hasPowerupDictionary = new Dictionary<int, Powerup>();
    }

    public void AddPlayer(int playerId, PlayerPowerup player)
    {
        playersDictionary.Add(playerId, player);
        hasPowerupDictionary.Add(playerId, Powerup.None);
    }

    public void CollectedPowerup(int playerId, Collider2D powerup)
    {
		// Only assign a powerup if a player doesn't already have one
        if (GetPowerupForPlayer(playerId) != Powerup.None)
        {
            return;
        }
        hasPowerupDictionary[playerId] = Powerup.JupiterJump;
        Destroy(powerup.gameObject);
    }

    public bool HasPowerup(int playerId)
    {
        return GetPowerupForPlayer(playerId) != Powerup.None;
    }

    public void UsePowerup(int playerId)
    {
        if (GetPowerupForPlayer(playerId) == Powerup.None)
        {
            Debug.LogError("playerid " + playerId + " trying to use powerup they don't have");
            return;
        }
        // Remove the powerup
        hasPowerupDictionary[playerId] = Powerup.None;
        // Iterate and use the powerup on every player
        foreach (KeyValuePair<int, PlayerPowerup> entry in playersDictionary)
        {
            entry.Value.JupiterJump(playerId);
        }
    }

    private Powerup GetPowerupForPlayer(int playerId)
    {
        Powerup hasPowerUp;
        if (hasPowerupDictionary.TryGetValue(playerId, out hasPowerUp) == false)
        {
            Debug.LogError("Player id " + playerId + " not registered with powerup controller");
        }
        return hasPowerUp;
    }
}
