using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStyle : MonoBehaviour
{
    public enum Character
    {
        GreyKnight,
        DullGoldKnight,
        GoldKnight
    }
    public SpriteRenderer head;
    public SpriteRenderer torso;
    public SpriteRenderer leftArm;
    public SpriteRenderer rightArm;
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;
    public Sprite[] headAssets;
    public Sprite[] torsoAssets;
    public Sprite[] leftArmAssets;
    public Sprite[] rightArmAssets;
    public Sprite[] leftLegAssets;
    public Sprite[] rightLegAssets;
    public Character characterStyle;

    public void SetCharacterStyle(Character characterStyle)
    {
        this.characterStyle = characterStyle;
		head.sprite = headAssets[(int) characterStyle];
		torso.sprite = torsoAssets[(int) characterStyle];
		leftArm.sprite = leftArmAssets[(int) characterStyle];
		rightArm.sprite = rightArmAssets[(int) characterStyle];
		leftLeg.sprite = leftLegAssets[(int) characterStyle];
		rightLeg.sprite = rightLegAssets[(int) characterStyle];		
    }
}
