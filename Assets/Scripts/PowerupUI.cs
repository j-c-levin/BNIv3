using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUI : MonoBehaviour
{
    public Sprite noPowerup;
    public Sprite jupiterJump;
    public Sprite Noodler;
    private Image powerupIcon;
    private Transform player;

    public void Start()
    {
        powerupIcon = GetComponentInChildren<Image>();
        powerupIcon.enabled = false;
        player = transform.parent.GetComponentInChildren<PlayerMovement>().transform;
    }
    use very similar techniques here to keep the offscreen ui on the border of the screen and enable/disable as the player becomes and loses invisibility
    public void Update()
    {
        transform.localPosition = player.localPosition;
    }

    public void CollectedPowerup(PlayerPowerup.Powerup powerup)
    {
        Sprite currentAbility = null;
        switch (powerup)
        {
            case PlayerPowerup.Powerup.JupiterJump:
                currentAbility = jupiterJump;
                break;
            case PlayerPowerup.Powerup.Noodler:
                currentAbility = Noodler;
                break;
            default:
                Debug.LogError("Unknown sprite for ability " + powerup);
                break;
        }
        powerupIcon.sprite = currentAbility;
        powerupIcon.enabled = true;
    }

    public void UsePowerup()
    {
        powerupIcon.sprite = noPowerup;
        powerupIcon.enabled = false;
    }
}
