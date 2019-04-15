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
	public float armor;
	public float damage;

	public CharacterAttributes()
	{
	}

	public CharacterAttributes(CharacterType type, float health, float speed, float armor, float damage)
	{
		this.health = health;
		this.speed = speed;
		this.armor = armor;
		this.damage = damage;
		this.type = type;
	}

	public void SetAttributes(CharacterAttributes attributes)
	{
		this.health = attributes.health;
		this.speed = attributes.speed;
		this.damage = attributes.damage;
		this.armor = attributes.armor;
		this.type = attributes.type;

		index.health = attributes.index.health;
		index.speed = attributes.index.speed;
		index.damage = attributes.index.damage;
		index.armor = attributes.index.armor;

		upgrade.healthCost = attributes.upgrade.healthCost;
		upgrade.speedCost = attributes.upgrade.speedCost;
		upgrade.damageCost = attributes.upgrade.damageCost;
		upgrade.armorCost = attributes.upgrade.armorCost;

	}
	public void SetAttributes(DefaultCharacterAttribute def)
	{
		this.health = def.health;
		this.speed = def.speed;
		this.damage = def.damage;
		this.armor = def.armor;
		this.type = def.type;

		upgrade.healthCost = def.healthCost;
		upgrade.speedCost = def.speedCost;
		upgrade.damageCost = def.damageCost;
		upgrade.armorCost = def.armorCost;
	}
	public CharacterAttributes GetAttributes()
	{
		return this;
	}


	public void Save()
	{
		PlayerPrefs.SetString(type + "", "recorded");
		PlayerPrefs.SetFloat(type + "_Health", Health);
		PlayerPrefs.SetFloat(type + "_Speed", Speed);
		PlayerPrefs.SetFloat(type + "_Damage", Damage);
		PlayerPrefs.SetFloat(type + "_Armor", Armor);

		PlayerPrefs.SetInt(type + "_HealthIndex", index.health);
		PlayerPrefs.SetInt(type + "_SpeedIndex", index.speed);
		PlayerPrefs.SetInt(type + "_DamageIndex", index.damage);
		PlayerPrefs.SetInt(type + "_ArmorIndex", index.armor);

		PlayerPrefs.SetFloat(type + "_HealthCost", upgrade.healthCost);
		PlayerPrefs.SetFloat(type + "_SpeedCost", upgrade.speedCost);
		PlayerPrefs.SetFloat(type + "_DamageCost", upgrade.damageCost);
		PlayerPrefs.SetFloat(type + "_ArmorCost", upgrade.armorCost);

	}

	public static CharacterAttributes Load(CharacterType type)
	{
		float _health = PlayerPrefs.GetFloat(type + "_Health");
		float _speed = PlayerPrefs.GetFloat(type + "_Speed");
		float _damage = PlayerPrefs.GetFloat(type + "_Damage");
		float _armor =  PlayerPrefs.GetFloat(type + "_Armor");

		int _indexhealth = PlayerPrefs.GetInt(type + "_HealthIndex");
		int _indexspeed = PlayerPrefs.GetInt(type + "_SpeedIndex");
		int _indexdamage = PlayerPrefs.GetInt(type + "_DamageIndex");
		int _indexarmor = PlayerPrefs.GetInt(type + "_ArmorIndex");

		float _healthCost = PlayerPrefs.GetFloat(type + "_HealthCost");
		float _speedCost = PlayerPrefs.GetFloat(type + "_SpeedCost");
		float _damageCost = PlayerPrefs.GetFloat(type + "_DamageCost");
		float _armorCost =  PlayerPrefs.GetFloat(type + "_ArmorCost");



		CharacterAttributes ca = new CharacterAttributes(type, _health, _speed, _damage, _armor);

		ca.index.health = _indexhealth;
		ca.index.speed = _indexspeed;
		ca.index.damage = _indexdamage;
		ca.index.armor = _indexarmor;

		ca.upgrade.healthCost = _healthCost;
		ca.upgrade.speedCost = _speedCost;
		ca.upgrade.damageCost = _damageCost;
		ca.upgrade.armorCost = _armorCost;

		return ca;

	}

	public float Health
	{
		get {

			return health;

		}
		set {
			this.health = value;
		}
	}
	public float Speed
	{
		get {return speed;}
		set {
			this.speed = value;
		}
	}
	public float Damage
	{
		get {return damage;}
		set {
			this.damage = value;
		}
	}
	public float Armor
	{
		get {return armor;}
		set {
			this.armor = value;
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
