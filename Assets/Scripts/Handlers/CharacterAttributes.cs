using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterAttributes
{
	public CharacterType type;
	public CharacterIndex index;
	public CharacterUpgradeCost upgrade;
	public float health;
	public float speed;
	public float stamina;
	public float damage;
	public float criticalHit;
	public float staminaRegen;

	public CharacterAttributes () { }

	public CharacterAttributes (CharacterType type, float health, float speed, float damage)
	{
		this.health = health;
		this.speed = speed;
		this.damage = damage;
		this.type = type;
	}

	public void SetAttributes (CharacterAttributes attributes)
	{
		this.health = attributes.health;
		this.speed = attributes.speed;
		this.damage = attributes.damage;
		this.type = attributes.type;
		this.stamina = attributes.stamina;
		this.criticalHit = attributes.criticalHit;
		this.staminaRegen = attributes.staminaRegen;

		index.health = attributes.index.health;
		index.speed = attributes.index.speed;
		index.damage = attributes.index.damage;

		upgrade.healthCost = attributes.upgrade.healthCost;
		upgrade.speedCost = attributes.upgrade.speedCost;
		upgrade.damageCost = attributes.upgrade.damageCost;
	}

	public void SetAttributes (string _id, CharacterAttributes attributes)
	{
		switch (_id)
		{
			case "Health":
				this.health = attributes.health;
				break;
			case "Damage":
				this.damage = attributes.damage;
				break;
			case "Stamina":
				this.stamina = attributes.stamina;
				break;
			case "type":
				this.type = attributes.type;
				break;
			case "CriticalHit":
				this.criticalHit = attributes.criticalHit;
				break;
			case "StaminaRegen":
				this.staminaRegen = attributes.staminaRegen;
				break;
		}
	}
	public void SetAttributes (CharacterStats stats)
	{
		this.type = stats.type;
		this.health = stats.health;
		this.damage = stats.damage;
		this.speed = stats.speed;
		this.stamina = stats.stamina;
		this.criticalHit = stats.criticalHit;
		this.staminaRegen = stats.staminaRegen;

	}
	public void SetAttributes (DefaultCharacterAttribute def)
	{
		this.health = def.health;
		this.speed = def.speed;
		this.damage = def.damage;
		this.type = def.type;

		upgrade.healthCost = def.healthCost;
		upgrade.speedCost = def.speedCost;
		upgrade.damageCost = def.damageCost;
		upgrade.armorCost = def.armorCost;
	}
	public CharacterAttributes GetAttributes ()
	{
		return this;
	}

	public void Save ()
	{
		PlayerPrefs.SetString (type + "", "recorded");
		PlayerPrefs.SetFloat (type + "_Health", Health);
		PlayerPrefs.SetFloat (type + "_Speed", Speed);
		PlayerPrefs.SetFloat (type + "_Damage", Damage);

		PlayerPrefs.SetInt (type + "_HealthIndex", index.health);
		PlayerPrefs.SetInt (type + "_SpeedIndex", index.speed);
		PlayerPrefs.SetInt (type + "_DamageIndex", index.damage);

		PlayerPrefs.SetFloat (type + "_HealthCost", upgrade.healthCost);
		PlayerPrefs.SetFloat (type + "_SpeedCost", upgrade.speedCost);
		PlayerPrefs.SetFloat (type + "_DamageCost", upgrade.damageCost);
		PlayerPrefs.SetFloat (type + "_ArmorCost", upgrade.armorCost);

	}

	public static CharacterAttributes Load (CharacterType type)
	{
		float _health = PlayerPrefs.GetFloat (type + "_Health");
		float _speed = PlayerPrefs.GetFloat (type + "_Speed");
		float _damage = PlayerPrefs.GetFloat (type + "_Damage");

		int _indexhealth = PlayerPrefs.GetInt (type + "_HealthIndex");
		int _indexspeed = PlayerPrefs.GetInt (type + "_SpeedIndex");
		int _indexdamage = PlayerPrefs.GetInt (type + "_DamageIndex");

		float _healthCost = PlayerPrefs.GetFloat (type + "_HealthCost");
		float _speedCost = PlayerPrefs.GetFloat (type + "_SpeedCost");
		float _damageCost = PlayerPrefs.GetFloat (type + "_DamageCost");
		float _armorCost = PlayerPrefs.GetFloat (type + "_ArmorCost");

		CharacterAttributes ca = new CharacterAttributes (type, _health, _speed, _damage);

		ca.index.health = _indexhealth;
		ca.index.speed = _indexspeed;
		ca.index.damage = _indexdamage;

		ca.upgrade.healthCost = _healthCost;
		ca.upgrade.speedCost = _speedCost;
		ca.upgrade.damageCost = _damageCost;
		ca.upgrade.armorCost = _armorCost;

		return ca;

	}

	public float Health
	{
		get
		{

			return health;

		}
		set
		{
			this.health = value;
		}
	}
	public float Speed
	{
		get { return speed; }
		set
		{
			this.speed = value;
		}
	}
	public float Damage
	{
		get { return damage; }
		set
		{
			this.damage = value;
		}
	}
}

public struct CharacterIndex
{
	public int health;
	public int speed;
	public int damage;
	public int armor;
}

public struct CharacterUpgradeCost
{
	public float healthCost;
	public float speedCost;
	public float damageCost;
	public float armorCost;
}

public enum CharacterType
{
	AURA_BLACKSWORD,
	HALLFRED_THORALDSON,
	FREYA_SKAAR,
}

public enum CharacterState
{
	IDLE,
	ATTACK,
	RUN,
}