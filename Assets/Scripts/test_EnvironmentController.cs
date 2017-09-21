using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_EnvironmentController : MonoBehaviour
{
    public EnvironmentController.environmentHazard hazard;
    public float timeBetweenHazard;
    public GameObject castleDropPlatform;
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
            case EnvironmentController.environmentHazard.FallingBlocks:
                CastleDropPlatform();
                break;
        }
    }

    private void CastleDropPlatform()
    {
        Vector3 spawnPosition = player.position;
        spawnPosition.y = 6f;
        spawnPosition.z = 1f;
        GameObject platform = Instantiate(castleDropPlatform, Vector2.zero, castleDropPlatform.transform.rotation);
        platform.transform.SetParent(mainCamera.transform);
        platform.transform.localPosition = spawnPosition;
        platform.GetComponent<castleDropPlatform>().endOfHazardDelegate = EndOfHazard;
    }

    private void EndOfHazard()
    {
        StartCoroutine("SpawnHazard");
    }
}
