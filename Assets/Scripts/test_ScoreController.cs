using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class test_ScoreController : MonoBehaviour, IScoreController
{
    public int deathPenalty;
    public int diamondValue;
    private int playerScore = 0;
    public void CollectedDiamond(int playerId, Collider2D diamond)
    {
        playerScore += diamondValue;
        Destroy(diamond.gameObject);
    }

    public void PlayerDeath(int playerId)
    {
        playerScore += deathPenalty;
    }
}
