using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreController  {
	void CollectedDiamond(int playerId, Collider2D diamond);
	void PlayerDeath(int playerId);
}
