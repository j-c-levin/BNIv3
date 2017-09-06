using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class SpawnController : MonoBehaviour, ISpawnController
{
    public GameObject playerPrefab;
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

    public void Start()
    {
        players = new Dictionary<int, GameObject>();
        spawn = GameObject.FindGameObjectWithTag("MainCamera");
        AirConsole.instance.onConnect += OnConnect;
    }

    public void SpawnPlayer(GameObject player)
    {
        player.transform.position = spawnPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private void OnConnect(int playerId)
    {
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        player.GetComponent<PlayerScore>().playerId = playerId;
        players.Add(playerId, player);
    }
}