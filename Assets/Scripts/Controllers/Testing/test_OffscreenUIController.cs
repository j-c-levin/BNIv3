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
    private float playerXEdge = 8f;
    private float playerYEdge = 4f;
    private test_SpawnController spawnController;

    public void Start()
    {
        spawnController = GetComponent<test_SpawnController>();
    }

    public void Update()
    {
        float percentageX = (cam.transform.position.x - spawnController.playerMovement.transform.position.x) / playerXEdge;
        float percentageY = (cam.transform.position.y - spawnController.playerMovement.transform.position.y) / playerYEdge;
        percentageX = Mathf.Clamp(percentageX, -1, 1);
        percentageY = Mathf.Clamp(percentageY, -1, 1);
        if (percentageX == 1)
        {
            offscreenIndicator.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (percentageX == -1)
        {
            offscreenIndicator.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (percentageY == 1)
        {
            offscreenIndicator.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (percentageY == -1)
        {
            offscreenIndicator.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        percentageX *= xClamp * -1;
        percentageY *= yClamp * -1;
        Vector2 newIndicatorPosition = new Vector2(percentageX, percentageY);
        offscreenIndicator.transform.localPosition = newIndicatorPosition;
    }

    public void PlayerOffscreen(int playerId, bool isOffscreen)
    {
        if (offscreenIndicator)
        {
            offscreenIndicator.enabled = isOffscreen;
        }
    }
}
