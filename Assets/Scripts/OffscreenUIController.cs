using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NDream.AirConsole;

public class OffscreenUIController : MonoBehaviour, IOffscreenUIController
{
    public Image offscreenIndicatorImage;
    private float playerEdgeHorizontal = 8;
    private float playerEdgeVertical = 4;
    private float indicatorHorizontalEdge = 550;
    private float indicatorVerticalEdge = 275;
    private SpawnController spawnController;
    private Camera gameCamera;
    private Canvas offscreenCanvas;
    private Dictionary<int, Image> offscreenIndicators;

    public void Start()
    {
		offscreenIndicators = new Dictionary<int, Image>();
        spawnController = GetComponent<SpawnController>();
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        offscreenCanvas = GameObject.FindGameObjectWithTag("OffscreenUI").GetComponent<Canvas>();
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    public void PlayerOffscreen(int playerId, bool isOffscreen)
    {
        offscreenIndicators[playerId].enabled = isOffscreen;
    }

    public void Update()
    {
        foreach (KeyValuePair<int, Image> entry in offscreenIndicators)
        {
            UpdateIndicator(entry);
        }
    }

    private void UpdateIndicator(KeyValuePair<int, Image> entry)
    {
        Transform player = spawnController.players[entry.Key].GetComponentInChildren<PlayerMovement>().transform;
        float percentageX = (gameCamera.transform.position.x - player.position.x) / playerEdgeHorizontal;
        float percentageY = (gameCamera.transform.position.y - player.position.y) / playerEdgeVertical;
        percentageX = Mathf.Clamp(percentageX, -1, 1);
        percentageY = Mathf.Clamp(percentageY, -1, 1);
        if (percentageX == 1)
        {
            entry.Value.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (percentageX == -1)
        {
            entry.Value.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (percentageY == 1)
        {
            entry.Value.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (percentageY == -1)
        {
            entry.Value.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        percentageX *= indicatorHorizontalEdge * -1;
        percentageY *= indicatorVerticalEdge * -1;
        Vector2 newIndicatorPosition = new Vector2(percentageX, percentageY);
        entry.Value.transform.localPosition = newIndicatorPosition;
    }

    private void OnConnect(int playerId)
    {
        Image indicator = Instantiate(offscreenIndicatorImage, transform.position, Quaternion.identity);
        indicator.transform.SetParent(offscreenCanvas.transform);
        offscreenIndicators.Add(playerId, indicator);
    }

    private void OnDisconnect(int playerId)
    {
        offscreenIndicators.Remove(playerId);
    }
}
