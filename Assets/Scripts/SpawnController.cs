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
    private GameObject spawn;
    private Vector3 spawnPosition
    {
        get
        {
            Vector3 spawnPosition = spawn.transform.position;
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
        spawn = GameObject.FindGameObjectWithTag("MainCamera");
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        ResetRace();
    }

    public void SpawnPlayer(GameObject player)
    {
        player.transform.position = spawnPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
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
    }

    private void OnConnect(int playerId)
    {
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        player.GetComponent<SpriteRenderer>().color = colours[colourNumber];
        colourNumber += 1;
        colourNumber %= colours.Length;
        player.GetComponent<PlayerScore>().playerId = playerId;
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