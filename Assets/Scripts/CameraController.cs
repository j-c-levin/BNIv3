using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    private CameraMovement gameCamera;
    private bool endOfRace;
    
    public void Start()
    {
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        gameCamera.destinationReachedDelegate = DestinationSwap;
        StartRace();
    }

    public void ResetRace()
    {
        gameCamera.transform.position = new Vector3(0, 0, gameCamera.transform.position.z);
        // Start the camera again
        StartRace();
    }

    private void StartRace()
    {
        gameCamera.SetTarget(target1);
        endOfRace = false;
    }

    private void DestinationSwap()
    {
        if (endOfRace == false)
        {
            gameCamera.SetTarget(target2);
            endOfRace = true;
        }
        else
        {
            GetComponent<UIController>().EndOfRace();
        }
    }
}
