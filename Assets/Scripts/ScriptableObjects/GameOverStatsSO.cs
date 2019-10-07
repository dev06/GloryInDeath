using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gameOverStats", menuName = "ScriptableObjects/GameOverStats")]
public class GameOverStatsSO: ScriptableObject
{
	public string timeSurvived;
	public int kills;
	public float damageDealt;
	public float damageTaken;
}
