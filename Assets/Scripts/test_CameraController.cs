using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_CameraController : MonoBehaviour {
	public GameObject target1;
	public GameObject target2;
	public CameraMovement gameCamera;

	private GameObject currentTarget;
	// Use this for initialization
	void Start () {
		gameCamera.destinationReachedDelegate = DestinationSwap; 
		gameCamera.SetTarget(target1);	
		currentTarget = target1;
	}

	void DestinationSwap() {
		GameObject target = (currentTarget == target1) ? target2 : target1;
		currentTarget = target;
		gameCamera.SetTarget(target);
	}
}
