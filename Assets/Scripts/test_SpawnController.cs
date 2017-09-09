using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_SpawnController : MonoBehaviour, ISpawnController
{
    public GameObject playerPrefab;
    public void SpawnPlayer(GameObject player)
    {
        Destroy(player);
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
}