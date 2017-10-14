using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facelaser : MonoBehaviour
{
    public float movementSpeed;
    public int castingPlayerId;
    private int playerLayer = 10;

    public void Update()
    {
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime, Space.Self);
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        bool isPlayerObject = collider.gameObject.layer == playerLayer;
        bool isOtherPlayer = collider.GetComponent<PlayerScore>().playerId != castingPlayerId;
        if (isPlayerObject && isOtherPlayer)
        {
            collider.GetComponent<DPlayerDeath>().TriggerDeath();
            Destroy(this.gameObject);
        }
    }

    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
