using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
	public GameObject target1;
	public GameObject target2;
	private CameraMovement gameCamera;
	// Use this for initialization
	void Start () {
		gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
		gameCamera.destinationReachedDelegate = DestinationSwap; 
		gameCamera.SetTarget(target1);	
	}

	void DestinationSwap() {
		gameCamera.SetTarget(target2);
	}
}
