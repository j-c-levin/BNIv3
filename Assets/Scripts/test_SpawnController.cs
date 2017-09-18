using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_SpawnController : MonoBehaviour, ISpawnController
{
    public GameObject playerPrefab;
    public GameObject player;
    public PlayerMovement playerMovement;
    public PlayerStyle.Character character;

    public void Start()
    {
        player.GetComponent<PlayerStyle>().SetCharacterStyle(character);
    }

    public void SpawnPlayer(GameObject player)
    {
        Destroy(player);
        this.player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        this.player.GetComponent<PlayerStyle>().SetCharacterStyle(character);
        playerMovement = this.player.GetComponentInChildren<PlayerMovement>();
    }
}