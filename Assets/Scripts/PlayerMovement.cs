using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float horizontalMovementSpeed;
    public float jumpSpeed;
    public float dropSpeed;
    private Rigidbody2D player;
    private enum JumpDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
    // Use this for initialization
    public void Start()
    {
        player = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            Debug.LogError("No rigidbody found on player");
        }
    }

    public void JumpLeft()
    {
        Jump((int)JumpDirection.LEFT);
    }

    public void JumpRight()
    {
        Jump((int)JumpDirection.RIGHT);
    }
    public void Drop()
    {
        player.velocity = new Vector2(0, -dropSpeed);
    }

    private void Jump(int direction)
    {
        player.velocity = new Vector2(horizontalMovementSpeed * direction, jumpSpeed);
    }
}
