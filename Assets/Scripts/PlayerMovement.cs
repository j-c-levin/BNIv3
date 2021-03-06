﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float horizontalMovementSpeed;
    public float jumpSpeed;
    private Rigidbody2D player;
    private enum JumpDirection
    {
        Left = -1,
        Right = 1,
        Straight = 0
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
        Jump((int)JumpDirection.Left);
    }

    public void JumpRight()
    {
        Jump((int)JumpDirection.Right);
    }
    
    public void JumpUp()
    {
        Jump((int)JumpDirection.Straight);
    }

    private void Jump(int direction)
    {
        player.velocity = new Vector2(horizontalMovementSpeed * direction, jumpSpeed);
    }
}
