using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleDropPlatform : MonoBehaviour
{
    public delegate void EndOfHazardDelegate();
    public EndOfHazardDelegate endOfHazardDelegate;
    public float dropSpeed;
    private bool drop = false;
    private Rigidbody2D platform;
    private float playerEdgeHorizontal = 8;
    private float indicatorHorizontalEdge = 550;
    private Transform player;

    public void Start()
    {
        SetIndicatorPosition();
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player.transform;
    }

    public void StartDrop()
    {
        platform = GetComponent<Rigidbody2D>();
        transform.SetParent(null);
        drop = true;
    }

    public void Update()
    {
        if (drop == false)
        {
            return;
        }
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - (dropSpeed * Time.deltaTime));
        platform.MovePosition(newPosition);
    }

    public void OnBecameInvisible()
    {
        if (endOfHazardDelegate == null)
        {
            Debug.LogError("No end of hazard delegate");
        }
        endOfHazardDelegate();
        Destroy(this.gameObject);
    }

    private void SetIndicatorPosition()
    {
        Transform gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        float percentageX = (gameCamera.transform.position.x - player.position.x) / playerEdgeHorizontal;
        percentageX = Mathf.Clamp(percentageX, -1, 1);
        percentageX *= indicatorHorizontalEdge * -1;
        float height = 273f;
        Vector2 newIndicatorPosition = new Vector2(percentageX, height);
        GetComponentInChildren<Image>().transform.localPosition = newIndicatorPosition;
    }
}
