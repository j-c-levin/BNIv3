using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_EnvironmentController : MonoBehaviour
{
    public EnvironmentController.environmentHazard hazard;
    public float timeBetweenHazard;
    public GameObject castleDropPlatform;
    public GameObject bombsAway;
    private Transform player;
    private Transform mainCamera;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        StartCoroutine("SpawnHazard");
    }

    private IEnumerator SpawnHazard()
    {
        yield return new WaitForSeconds(timeBetweenHazard);
        switch (hazard)
        {
            case EnvironmentController.environmentHazard.CastleDropPlatform:
                CastleDropPlatform();
                break;
            case EnvironmentController.environmentHazard.BombsAway:
                BombsAway();
                break;
        }
    }

    private void BombsAway()
    {
        GameObject bomb = Instantiate(bombsAway, Vector2.zero, castleDropPlatform.transform.rotation);
        bomb.transform.SetParent(mainCamera.transform);
        Vector3 spawnPosition = new Vector3(-10f, -2, 1f);
        bomb.transform.localPosition = spawnPosition;
        bomb.GetComponent<BombsAway>().endOfHazardDelegate = EndOfHazard;
    }

    private void CastleDropPlatform()
    {
        GameObject platform = Instantiate(castleDropPlatform, Vector2.zero, castleDropPlatform.transform.rotation);
        platform.transform.SetParent(mainCamera.transform);
        Vector3 spawnPosition = new Vector3(player.position.x, 6f, 1f);
        platform.transform.localPosition = spawnPosition;
        platform.GetComponent<CastleDropPlatform>().endOfHazardDelegate = EndOfHazard;
        platform.GetComponent<CastleDropPlatform>().SetPlayer(player.gameObject);
    }

    private void EndOfHazard()
    {
        StartCoroutine("SpawnHazard");
    }
}
