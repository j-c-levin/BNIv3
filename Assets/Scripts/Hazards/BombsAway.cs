using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BombsAway : MonoBehaviour
{
    public EnvironmentController.EndOfHazardDelegate endOfHazardDelegate;
    public float duration;
    public float jumpPower;
    public int numberOfJumps;
    public float explosionForce;
    public float explosionRadius;
    private Vector2 target;
    private Tween movement;
    private Transform player;
    private float playerEdgeHorizontal = 8;
    private float playerEdgeVertical = 4;
    private float indicatorHorizontalEdge = 550;
    private float indicatorVerticalEdge = 275;
    private Image indicator;
    private Transform gameCamera;
    private Vector3 lastCameraPosition;

    public void Start()
    {
        SetIndicatorPosition();
    }

    public void Update()
    {
        float cameraMovement = gameCamera.position.y - lastCameraPosition.y; 
        lastCameraPosition = gameCamera.position;
        indicator.transform.Translate(0, -cameraMovement * 10, 0);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player.transform;
    }

    public void LaunchBomb()
    {
        Rigidbody2D bomb = GetComponent<Rigidbody2D>();
        movement = bomb.DOJump(target, jumpPower, numberOfJumps, duration).OnComplete(OnTweenComplete);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Rigidbody2D player = collider.GetComponent<Rigidbody2D>();
        Vector2 explosionDirection = transform.position - collider.transform.position;
        explosionDirection *= explosionForce * -1;
        explosionDirection *= (movement.IsPlaying()) ? 3 : 1;
        player.AddForce(explosionDirection, ForceMode2D.Impulse);
        if (movement.IsPlaying() == true)
        {
            if (endOfHazardDelegate == null)
            {
                Debug.LogError("No end of hazard delegate");
            }
            else
            {
                endOfHazardDelegate();
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTweenComplete()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = explosionRadius;
        StartCoroutine("WaitAFrame");
    }

    private IEnumerator WaitAFrame()
    {
        // Two 'wait for end of frame' because the physics trigger only occurs on the next frame
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (endOfHazardDelegate == null)
        {
            Debug.LogError("No end of hazard delegate");
        }
        else
        {
            endOfHazardDelegate();
        }
        Destroy(this.gameObject);
    }

    private void SetIndicatorPosition()
    {
        indicator = GetComponentInChildren<Image>();
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        lastCameraPosition = gameCamera.transform.position;
        float percentageX = (gameCamera.position.x - player.position.x) / playerEdgeHorizontal;
        float percentageY = (gameCamera.position.y - player.position.y) / playerEdgeVertical;
        percentageX = Mathf.Clamp(percentageX, -1, 1) * indicatorHorizontalEdge * -1;
        percentageY = Mathf.Clamp(percentageY, -1, 1) * indicatorVerticalEdge * -1;
        Vector2 newIndicatorPosition = new Vector2(percentageX, percentageY);
        indicator.transform.localPosition = newIndicatorPosition;
        target = player.position;
        LaunchBomb();
    }
}
