using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	private ISpawnController spawn;
    private float deathCounter = 0;
	private float deathCounterMax = 100;
	private float deathCounterMin = 0;
	private float deathCounterAddition = 2;
	private float deathCounterReduction = 1;
    private bool isOffScreen = false;

    public void Start()
    {
		spawn = GameObject.FindGameObjectWithTag("GameController").GetComponent<ISpawnController>();
    }

    public void FixedUpdate()
    {
        if (isOffScreen)
        {
            deathCounter += deathCounterAddition;
        }
        else
        {
            deathCounter -= deathCounterReduction;
        }
        deathCounter = Mathf.Clamp(deathCounter, deathCounterMin, deathCounterMax);
        if (deathCounter == deathCounterMax)
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
    }
}
