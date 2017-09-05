using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int playerId;
    private IScoreController scoreController;
    private int diamondLayer = 8;
    public void Start()
    {
        scoreController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IScoreController>();
        if (scoreController == null)
        {
            Debug.LogError("Score controller not found");
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == diamondLayer)
        {
            scoreController.CollectedDiamond(playerId, collider);
        }
    }

}
