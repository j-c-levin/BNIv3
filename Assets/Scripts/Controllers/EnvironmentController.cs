using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public enum environmentHazard
    {
        CastleDropPlatform,
        BombsAway
    }
    public delegate void EndOfHazardDelegate();
    public float timeBetweenHazards;
    public GameObject castleDropPlatform;
    public GameObject bombsAway;
    private Transform mainCamera;
    private SpawnController spawnController;
    private bool hazardIsRunning = false;

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
        hazardIsRunning = true;
        yield return new WaitForSeconds(timeBetweenHazards);
        if (GetComponent<MovementController>().isRaceRunning == false)
        {
            yield return null;
        }
        int numberOfHazards = Enum.GetNames(typeof(environmentHazard)).Length;
        environmentHazard currentHazard = (environmentHazard)UnityEngine.Random.Range(0, numberOfHazards);
        // environmentHazard currentHazard = environmentHazard.BombsAway;
        switch (currentHazard)
        {
            case environmentHazard.CastleDropPlatform:
                CastleDropPlatform();
                break;
            case environmentHazard.BombsAway:
                BombsAway();
                break;
            default:
                Debug.LogError("unimplement hazard: " + currentHazard);
                break;
        }
        hazardIsRunning = false;
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
            platform.transform.SetParent(null);
        }
    }

    private void BombsAway()
    {
        foreach (KeyValuePair<int, GameObject> entry in spawnController.players)
        {
            GameObject player = entry.Value.GetComponentInChildren<PlayerMovement>().gameObject;
            GameObject hazard = Instantiate(bombsAway, Vector2.zero, bombsAway.transform.rotation);
            hazard.transform.SetParent(mainCamera.transform);
            hazard.GetComponent<BombsAway>().endOfHazardDelegate = EndOfHazard;
            hazard.GetComponent<BombsAway>().SetPlayer(player);
            float leftOrRightSide = (entry.Key % 2 == 0) ? -11 : 11;
            Vector3 spawnPosition = new Vector3(
                mainCamera.transform.position.x + leftOrRightSide,
                mainCamera.transform.position.y - 3,
                entry.Value.transform.position.z);
            hazard.transform.position = spawnPosition;
            hazard.transform.SetParent(null);
        }
    }

    private void EndOfHazard()
    {
        if (hazardIsRunning == false)
        {
            StartCoroutine("SelectHazard");
        }
    }
}
