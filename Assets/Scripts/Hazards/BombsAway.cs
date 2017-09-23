using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombsAway : MonoBehaviour
{
    public Vector2 target;
    public float duration;
    public float jumpPower;
    public int numberOfJumps;
    public float explosionForce;
	public float explosionRadius;
    private Tween movement;

    public void Start()
    {
        Rigidbody2D bomb = GetComponent<Rigidbody2D>();
        movement = bomb.DOJump(target, jumpPower, numberOfJumps, duration).OnComplete(OnTweenComplete);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Rigidbody2D player = collider.GetComponent<Rigidbody2D>();
        Vector2 explosionDirection = transform.position - collider.transform.position;
        explosionDirection *= explosionForce * -1;
        player.AddForceAtPosition(explosionDirection, transform.position, ForceMode2D.Impulse);
        if (movement.IsPlaying() == true)
        {
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
        Destroy(this.gameObject);	
    }
}
