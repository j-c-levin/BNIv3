using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class PowerupController : MonoBehaviour, IPowerupController
{
    private SpawnController spawnController;
    private Dictionary<int, PlayerPowerup.Powerup> hasPowerupDictionary;

    public void Start()
    {
        spawnController = GetComponent<SpawnController>();
        hasPowerupDictionary = new Dictionary<int, PlayerPowerup.Powerup>();
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    public PlayerPowerup.Powerup CollectedPowerup(int playerId, Collider2D powerup)
    {
        // Only assign a powerup if a player doesn't already have one
        if (GetPowerupForPlayer(playerId) != PlayerPowerup.Powerup.None)
        {
            return PlayerPowerup.Powerup.None;
        }
        int lengthOfPowerupEnum = Enum.GetNames(typeof(PlayerPowerup.Powerup)).Length;
        int ability = UnityEngine.Random.Range(1, lengthOfPowerupEnum);
        hasPowerupDictionary[playerId] = (PlayerPowerup.Powerup)ability;
        Destroy(powerup.gameObject);
        return (PlayerPowerup.Powerup)ability;
    }

    public bool HasPowerup(int playerId)
    {
        return GetPowerupForPlayer(playerId) != PlayerPowerup.Powerup.None;
    }

    public void UsePowerup(int playerId)
    {
        PlayerPowerup.Powerup power = GetPowerupForPlayer(playerId);
        if (power == PlayerPowerup.Powerup.None)
        {
            Debug.LogError("player id " + playerId + " trying to use powerup they don't have");
            return;
        }
        // Remove the powerup
        hasPowerupDictionary[playerId] = PlayerPowerup.Powerup.None;
        // Iterate and let each player handle how the power up affects it
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            entry.Value.GetComponentInChildren<PlayerPowerup>().UsePowerup(playerId, power);
        }
    }

    private PlayerPowerup.Powerup GetPowerupForPlayer(int playerId)
    {
        PlayerPowerup.Powerup hasPowerUp;
        if (hasPowerupDictionary.TryGetValue(playerId, out hasPowerUp) == false)
        {
            Debug.LogError("Player id " + playerId + " not registered with powerup controller");
        }
        return hasPowerUp;
    }

    private void OnConnect(int playerId)
    {
        hasPowerupDictionary.Add(playerId, PlayerPowerup.Powerup.None);
    }

    private void OnDisconnect(int playerId) {
        hasPowerupDictionary.Remove(playerId);
    }
}
