using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private ISpawnController spawn;
    private IScoreController scoreController;
    private float deathCounter = 0;
    private float deathDurationMax = 100;
    private float deathDurationMin = 0;
    private float deathDurationAddition = 2;
    private float deathDurationReduction = 1;
    private bool isOffScreen = false;

    public void Start()
    {
        spawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<ISpawnController>();
        if (spawn == null)
        {
            Debug.Log("Spawn controller not found");
        }
        scoreController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IScoreController>();
        if (scoreController == null)
        {
            Debug.LogError("Score controller not found");
        }
    }

    public void FixedUpdate()
    {
        if (isOffScreen)
        {
            deathCounter += deathDurationAddition;
        }
        else
        {
            deathCounter -= deathDurationReduction;
        }
        deathCounter = Mathf.Clamp(deathCounter, deathDurationMin, deathDurationMax);
        if (deathCounter == deathDurationMax)
        {
            TriggerDeath();
        }
    }

    public void OnBecameInvisible()
    {
        isOffScreen = true;
    }

    public void OnBecameVisible()
    {
        isOffScreen = false;
    }

    public void TriggerDeath()
    {
        deathCounter = 0;
        spawn.SpawnPlayer(this.gameObject);
        int playerId = GetComponent<PlayerScore>().playerId;
        scoreController.PlayerDeath(playerId);
    }
}
