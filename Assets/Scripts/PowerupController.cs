using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class PowerupController : MonoBehaviour, IPowerupController
{
    private enum Powerup
    {
        None,
        JupiterJump
    }
    private Dictionary<int, GameObject> playersDictionary;
    private Dictionary<int, Powerup> hasPowerupDictionary;

    public void Start()
    {
        playersDictionary = GetComponent<SpawnController>().players;
        hasPowerupDictionary = new Dictionary<int, Powerup>();
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    public void CollectedPowerup(int playerId, Collider2D powerup)
    {
        // Only assign a powerup if a player doesn't already have one
        if (GetPowerupForPlayer(playerId) != Powerup.None)
        {
            return;
        }
        int lengthOfPowerupEnum = Enum.GetNames(typeof(Powerup)).Length;
        int ability = UnityEngine.Random.Range(1, lengthOfPowerupEnum);
        hasPowerupDictionary[playerId] = (Powerup)ability;
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
        foreach (KeyValuePair<int, GameObject> entry in playersDictionary)
        {
            entry.Value.GetComponent<PlayerPowerup>().JupiterJump(playerId);
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

    private void OnConnect(int playerId)
    {
        hasPowerupDictionary.Add(playerId, Powerup.None);
    }

    private void OnDisconnect(int playerId) {
        hasPowerupDictionary.Remove(playerId);
    }
}
