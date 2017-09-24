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

    public void Start()
    {
        SetIndicatorPosition();
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
        Image indicator = GetComponentInChildren<Image>();
        float canvasWidth = 640f;
        float canvasHeight = 355f;
        float randomArea = 0.5f;
        float randomX = Random.Range(-randomArea, randomArea);
        float randomXPosition = randomX * Screen.width;
        float randomY = Random.Range(-randomArea, randomArea);
        float randomYPosition = randomY * Screen.height;
        Vector2 indicatorTarget = new Vector2(randomXPosition, randomYPosition);
        indicator.transform.localPosition = indicatorTarget;
        target = new Vector2(10 * randomX, 5 * randomY);
        LaunchBomb();
    }
}
