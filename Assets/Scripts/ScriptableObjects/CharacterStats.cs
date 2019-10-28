using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "character", menuName = "ScriptableObjects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
	public CharacterType type;
	public float health;
	public float speed;
	public float damage;
	public float stamina;
	public float criticalHit;
	public float staminaRegen;
}