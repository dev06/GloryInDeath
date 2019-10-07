using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStats: ScriptableObject
{
	public EnemyType type;
	public float health;
	public float damage;
	public float healthMult;
	public float damageMult;
}

