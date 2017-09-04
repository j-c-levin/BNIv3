using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour, ISpawnController
{
    private GameObject spawn;

    public void Start()
    {
		spawn = GameObject.FindGameObjectWithTag("MainCamera");
    }
    
    public void SpawnPlayer(GameObject player)
    {
        Vector3 spawnPosition = spawn.transform.position;
        spawnPosition.z = 0;
        player.transform.position = spawnPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}