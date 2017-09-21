using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public enum environmentHazard
    {
        CastleDropPlatform
    }
    public float timeBetweenHazards;
    public GameObject castleDropPlatform;
    private Transform mainCamera;
    private SpawnController spawnController;

    public void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        spawnController = GetComponent<SpawnController>();
        if (spawnController == null)
        {
            Debug.LogError("No spawn controller attached");
        }
    }

    public void StartHazards()
    {
        StartCoroutine("SelectHazard");
    }

    public void StopHazards()
    {
        StopCoroutine("SelectHazard");
    }

    private IEnumerator SelectHazard()
    {
        yield return new WaitForSeconds(timeBetweenHazards);
        if (GetComponent<MovementController>().isRaceRunning == false)
        {
            yield return null;
        }
        int numberOfHazards = Enum.GetNames(typeof(environmentHazard)).Length;
        environmentHazard currentHazard = (environmentHazard)UnityEngine.Random.Range(0, numberOfHazards);
        switch (currentHazard)
        {
            case environmentHazard.CastleDropPlatform:
                CastleDropPlatform();
                break;
        }
    }

    private void CastleDropPlatform()
    {
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            GameObject player = entry.Value.GetComponentInChildren<PlayerMovement>().gameObject;
            GameObject platform = Instantiate(castleDropPlatform, Vector2.zero, castleDropPlatform.transform.rotation);
            platform.transform.SetParent(mainCamera.transform);
            platform.GetComponent<CastleDropPlatform>().endOfHazardDelegate = EndOfHazard;
            platform.GetComponent<CastleDropPlatform>().SetPlayer(player);
            float dropHeight = mainCamera.transform.position.y + 6f;
            float spawnDepth = entry.Value.transform.position.z;
            Vector3 spawnPosition = new Vector3(player.transform.position.x, dropHeight, spawnDepth);
            platform.transform.position = spawnPosition;
        }
    }

    private void EndOfHazard()
    {
        StartCoroutine("SelectHazard");
    }
}
