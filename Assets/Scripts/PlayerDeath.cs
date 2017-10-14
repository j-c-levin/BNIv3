using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPlayerDeath : MonoBehaviour
{
    public float deathTimeout;
    private float deathCounter = 0;
    private bool isOffScreen = false;
    private ISpawnController spawn;
    private IScoreController scoreController;
    private IOffscreenUIController offscreenUIController;
    private int playerId;

    public void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        spawn = controller.GetComponent<ISpawnController>();
        if (spawn == null)
        {
            Debug.Log("Spawn controller not found");
        }
        // scoreController = controller.GetComponent<IScoreController>();
        // if (scoreController == null)
        // {
        //     Debug.LogError("Score controller not found");
        // }
        // offscreenUIController = controller.GetComponent<IOffscreenUIController>();
        // if (offscreenUIController == null)
        // {
        //     Debug.LogError("Offscreen UI Controller not found");
        // }
        // playerId = GetComponent<PlayerScore>().playerId;
    }

    public void FixedUpdate()
    {
        if (isOffScreen)
        {
            deathCounter += Time.deltaTime;
        }
        else
        {
            deathCounter -= Time.deltaTime;
        }
        deathCounter = Mathf.Clamp(deathCounter, 0, deathTimeout);
        if (deathCounter == deathTimeout)
        {
            TriggerDeath();
        }
    }

    public void OnBecameInvisible()
    {
        isOffScreen = true;
        // offscreenUIController.PlayerOffscreen(playerId, isOffScreen);
    }

    public void OnBecameVisible()
    {
        isOffScreen = false;
        // offscreenUIController.PlayerOffscreen(playerId, isOffScreen);
    }

    public void TriggerDeath()
    {
        deathCounter = 0;
        spawn.SpawnPlayer(transform.parent.gameObject);
        scoreController.PlayerDeath(playerId);
    }
}
