using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_OffscreenUIController : MonoBehaviour, IOffscreenUIController
{
    public Camera cam;
    public Image offscreenIndicator;
    public float xClamp;
    public float yClamp;
    private test_SpawnController spawnController;

    public void Start()
    {
        spawnController = GetComponent<test_SpawnController>();
    }

    public void Update()
    {
        offscreenIndicator.rectTransform.localPosition = cam.WorldToViewportPoint(spawnController.playerMovement.transform.position);
    }

    public void PlayerOffscreen(int playerId, bool isOffscreen)
    {
        // offscreenIndicator.enabled = isOffscreen;
    }
}
