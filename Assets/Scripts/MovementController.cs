using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class MovementController : MonoBehaviour
{
    public PlayerMovement player;
    private bool leftButton = false;
    private bool rightButton = false;
    void Start()
    {
        AirConsole.instance.onMessage += OnMessage;
    }

    void Update()
    {
        bool drop = leftButton && rightButton;
        if (drop)
        {
            player.Drop();
        }
        else
        {
            if (rightButton)
            {
                player.JumpRight();
            }
            else if (leftButton)
            {
                player.JumpLeft();
            }
        }
        leftButton = false;
        rightButton = false;
    }

    void OnMessage(int from, JToken data)
    {
        string name = (string)data["element"];
        if (name == "Null")
        {
            return;
        }
        if (name == "left")
        {
            leftButton = true;
        }
        else
        {
            rightButton = true;
        }
    }
}
