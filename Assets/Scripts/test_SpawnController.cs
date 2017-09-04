using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_SpawnController : MonoBehaviour, ISpawnController
{
    public void SpawnPlayer(GameObject player)
    {
        player.transform.position = Vector3.zero;
    }
}