using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Test_InputInitiator : MonoBehaviour
{
    private class KeyboardInput
    {
        public string element;
        public KeyboardInputData data;
    }
    private class KeyboardInputData
    {
        public bool pressed;
    }
    public GameObject player;
    private InputController inputController;
    private bool readyToJump;

    public void Start()
    {
        inputController = GetComponent<InputController>();
        inputController.AddPlayer(0, player);
    }

    public void Update()
    {
        KeyboardInput newInput = new KeyboardInput();
        newInput.element = "left";
        newInput.data = new KeyboardInputData();
        newInput.data.pressed = false;

        bool special = Input.GetKey(KeyCode.Space);

        if (special)
        {
            newInput.data.pressed = true;
            JToken outputOne = JToken.Parse(JsonConvert.SerializeObject(newInput));
            inputController.OnMessage(0, outputOne);
            newInput = new KeyboardInput();
            newInput.element = "right";
            newInput.data = new KeyboardInputData();
            newInput.data.pressed = true;
            outputOne = JToken.Parse(JsonConvert.SerializeObject(newInput));
            inputController.OnMessage(0, outputOne);
            return;
        }

        float movement = Input.GetAxisRaw("Horizontal");
        if (readyToJump && movement == 0)
        {
            // not moving, do nothing
            return;
        }
        if (readyToJump == false && movement == 0)
        {
            readyToJump = true;
            return;
        }
        if (readyToJump == false)
        {
            return;
        }
        if (movement > 0)
        {
            newInput.element = "right";
            newInput.data.pressed = true;
        }
        else if (movement < 0)
        {
            newInput.element = "left";
            newInput.data.pressed = true;
        }
        readyToJump = false;

        JToken output = JToken.Parse(JsonConvert.SerializeObject(newInput));
        inputController.OnMessage(0, output);
    }
}
