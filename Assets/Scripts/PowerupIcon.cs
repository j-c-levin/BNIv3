using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupIcon : MonoBehaviour
{
    public Sprite[] powerupSprites;
    public PlayerPowerup.Powerup powerup;

    void OnEnable()
    {
        int lengthOfPowerupEnum = Enum.GetNames(typeof(PlayerPowerup.Powerup)).Length;
        PlayerPowerup.Powerup power = (PlayerPowerup.Powerup)UnityEngine.Random.Range(0, lengthOfPowerupEnum);
        GetComponent<SpriteRenderer>().sprite = powerupSprites[(int)power];
        powerup = power;
    }
}