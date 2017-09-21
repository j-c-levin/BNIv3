using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class castleDropPlatform : MonoBehaviour
{
    public delegate void EndOfHazardDelegate();

    public EndOfHazardDelegate endOfHazardDelegate;
    public float dropSpeed;
    private bool drop = false;
    private Rigidbody2D platform;

    public void StartDrop()
    {
        platform = GetComponent<Rigidbody2D>();
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
}
