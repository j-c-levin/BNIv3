using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class SpawnController : MonoBehaviour, ISpawnController
{
    public GameObject playerPrefab;
    public GameObject diamonds;
    public GameObject powerups;
    public Dictionary<int, GameObject> players;
    private GameObject gameCamera;
    private Vector3 spawnPosition
    {
        get
        {
            Vector3 spawnPosition = gameCamera.transform.position;
            spawnPosition.z = 0;
            return spawnPosition;
        }
    }
    private Color[] colours = new Color[] { Color.red, Color.cyan, Color.green, Color.magenta, Color.yellow, Color.white, Color.black, Color.gray };
    private int colourNumber = 0;
    private GameObject spawnedDiamonds;
    private GameObject spawnedPowerups;

    public void Start()
    {
        players = new Dictionary<int, GameObject>();
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        ResetRace();
    }

    public void SpawnPlayer(GameObject player)
    {
        int playerId = player.GetComponentInChildren<PlayerScore>().playerId;
        Color colour = player.GetComponentInChildren<SpriteRenderer>().color;
        Destroy(player);
        players[playerId] = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        players[playerId].GetComponentInChildren<PlayerScore>().playerId = playerId;
        ColourPlayer(players[playerId], colour);
    }

    public void ResetRace()
    {
        if (spawnedDiamonds != null)
        {
            Destroy(spawnedDiamonds.gameObject);
        }
        if (spawnedPowerups != null)
        {
            Destroy(spawnedPowerups);
        }
        spawnedDiamonds = Instantiate(diamonds, diamonds.transform.position, Quaternion.identity);
        spawnedPowerups = Instantiate(powerups, powerups.transform.position, Quaternion.identity);
        List<GameObject> temp = new List<GameObject>();
        foreach (KeyValuePair<int, GameObject> entry in players)
        {
            temp.Add(entry.Value);
        }
        foreach (GameObject player in temp)
        {
            SpawnPlayer(player);
        }
    }

    private void ColourPlayer(GameObject player, Color colour)
    {
        foreach (SpriteRenderer s in player.GetComponentsInChildren<SpriteRenderer>())
        {
            s.color = colour;
        }
    }

    private void OnConnect(int playerId)
    {
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        ColourPlayer(player, colours[colourNumber]);
        Debug.Log("Player: " + playerId + " is colour: " + colours[colourNumber]);
        colourNumber += 1;
        colourNumber %= colours.Length;
        player.GetComponentInChildren<PlayerScore>().playerId = playerId;
        players.Add(playerId, player);
    }

    private void OnDisconnect(int playerId)
    {
        GameObject player;
        if (players.TryGetValue(playerId, out player) == false)
        {
            Debug.Log("Cannot find playerId " + playerId + " to destroy");
        }
        Destroy(player);
        players.Remove(playerId);
    }
}