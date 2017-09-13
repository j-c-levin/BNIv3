using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_RaceStartController : MonoBehaviour
{
    public float neutralForce;
    public float rotationForce;
    public float raceStartHeight;
    public float readyUpTimerDuration;
    private float currentReadyUpTimer = 0;
    private PlayerMovement player;
    private bool readyToJump = true;
    bool isReadyForRace = false;

    void Start()
    {
        GetComponent<test_MovementController>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Movement();
        PrepareForRace();
    }

    private void PrepareForRace()
    {
        if (isReadyForRace)
        {
            float distanceFromStartHeight = raceStartHeight - player.transform.position.y;
            float upwardForce = neutralForce * ((distanceFromStartHeight > 0) ? 1: 0f);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upwardForce);
            float rotationFromNeutral = (player.transform.rotation.eulerAngles.z <= 180) ? -player.transform.rotation.eulerAngles.z : 360 - player.transform.rotation.eulerAngles.z;
            float torqueForce = rotationForce * rotationFromNeutral;
            player.GetComponent<Rigidbody2D>().AddTorque(torqueForce, ForceMode2D.Force);
            currentReadyUpTimer += Time.deltaTime;
        }
        else
        {
            currentReadyUpTimer -= Time.deltaTime;
        }
        currentReadyUpTimer = Mathf.Clamp(currentReadyUpTimer, 0, readyUpTimerDuration);
        if (currentReadyUpTimer == readyUpTimerDuration)
        {
            StartRace();
        }
    }

    private void StartRace()
    {
        GetComponent<test_MovementController>().enabled = true;
        player.GetComponent<Rigidbody2D>().simulated = true;
        player.GetComponent<PlayerMovement>().JumpUp();
        this.enabled = false;
    }

    private void Movement()
    {
        bool readyUp = Input.GetKey(KeyCode.Space);
        float movement = Input.GetAxisRaw("Horizontal");
        if (readyToJump && movement == 0 && readyUp == false)
        {
            // not moving, do nothing
            return;
        }
        if (readyToJump == false && movement == 0 && readyUp == false)
        {
            isReadyForRace = false;
            readyToJump = true;
            return;
        }
        if (readyToJump == false)
        {
            return;
        }
        if (readyUp)
        {
            // prepare for race
            isReadyForRace = true;
        }
        if (movement > 0)
        {
            player.JumpRight();
        }
        else if (movement < 0)
        {
            player.JumpLeft();
        }
        readyToJump = false;
    }
}
