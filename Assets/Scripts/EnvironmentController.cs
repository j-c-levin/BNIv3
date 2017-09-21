using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public enum environmentHazard
    {
        FallingBlocks
    }

    public float timeBetweenHazards;

    public void StartHazards()
    {
        StartCoroutine("SelectHazard");
    }

    private IEnumerator SelectHazard()
    {
        yield return new WaitForSeconds(timeBetweenHazards);
        int numberOfHazards = Enum.GetNames(typeof(environmentHazard)).Length;
        // environmentHazard currentHazard = (environmentHazard)UnityEngine.Random.Range(0, numberOfHazards);
        // switch (currentHazard)
        // {

        // }
    }
}
