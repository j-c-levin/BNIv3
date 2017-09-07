using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;

public class ScoreController : MonoBehaviour, IScoreController
{
    public Dictionary<int, int> scores;
    public int deathPenalty;
    public int diamondValue;

    public void Start()
    {
        scores = new Dictionary<int, int>();
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }
	
    public void CollectedDiamond(int playerId, Collider2D diamond)
    {
        ModifyPlayerScore(playerId, diamondValue);
        Destroy(diamond.gameObject);
    }

    public void PlayerDeath(int playerId)
    {        
		ModifyPlayerScore(playerId, deathPenalty);
    }

    private void ModifyPlayerScore(int playerId, int modifier)
    {
		int playerScore;
		if(scores.TryGetValue(playerId, out playerScore) == false) {
			Debug.LogError("Player id " + playerId + " not registered with score controller");
		}
        playerScore += modifier;
        scores[playerId] = playerScore;
    }    
	
	private void OnConnect(int playerId)
    {
        scores.Add(playerId, 0);
    }

    private void OnDisconnect(int playerId)
    {
        scores.Remove(playerId);
    }
}
