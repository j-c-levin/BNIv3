using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_RaceStartController : MonoBehaviour
{
    public PlayerMovement player;
    public float raceStartHeight;
    public float raceStartLerpSpeed;
    public float raceStartCounterMax;
    public float raceStartCounterMin;
    public float raceStartModifier;
    private float currentRaceStartCount;
    private bool readyToJump = true;
    bool isReadyForRace = false;

    void Start()
    {
        GetComponent<test_MovementController>().enabled = false;
        currentRaceStartCount = 0;
    }

    void Update()
    {
        Movement();
        PrepareForRace();
    }

    private void PrepareForRace()
    {
        player.GetComponent<Rigidbody2D>().simulated = !isReadyForRace;
        if (isReadyForRace)
        {
            Vector2 startPosition = new Vector2(player.transform.position.x, raceStartHeight);
            player.transform.position = Vector2.Lerp(player.transform.position, startPosition, raceStartLerpSpeed);
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.identity, raceStartLerpSpeed);
            currentRaceStartCount += raceStartModifier * Time.deltaTime;
        }
        else
        {
            currentRaceStartCount -= raceStartModifier * Time.deltaTime;
        }
        currentRaceStartCount = Mathf.Clamp(currentRaceStartCount, raceStartCounterMin, raceStartCounterMax);
        if (currentRaceStartCount == raceStartCounterMax)
        {
            StartRace();
        }
    }

    private void StartRace()
    {
        GetComponent<test_MovementController>().enabled = true;
        player.GetComponent<Rigidbody2D>().simulated = true;
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
