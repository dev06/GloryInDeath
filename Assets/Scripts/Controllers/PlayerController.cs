using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLibrary
{
	public static CharacterAttributes CHAR_AURA_BLACKSWORD = new CharacterAttributes(100, 5, 2, 3);
	public static CharacterAttributes CHAR_HALLFRED_THORALDSON = new CharacterAttributes(100, 2, 3, 4);
	public static CharacterAttributes CHAR_FREYA_SKAAR = new CharacterAttributes(10, 5, 4, 5);

}

public class CharacterAttributes
{
	public float health;
	public float speed;
	public float armor;
	public float damage;

	public CharacterAttributes(float health, float speed, float armor, float damage)
	{
		this.health = health;
		this.speed = speed;
		this.armor = armor;
		this.damage = damage;
	}
}

[System.Serializable]
public class Character
{
	public enum CharacterType
	{
		AURA_BLACKSWORD,
		HALLFRED_THORALDSON,
		FREYA_SKAAR,
	}

	public CharacterType type;
	public CharacterStats stats;
	public CharacterInfo info;


	public void Init()
	{
		switch (type)
		{
			case CharacterType.AURA_BLACKSWORD:
			{
				stats.health = CharacterLibrary.CHAR_AURA_BLACKSWORD.health;
				stats.speed = CharacterLibrary.CHAR_AURA_BLACKSWORD.speed;
				stats.damage = CharacterLibrary.CHAR_AURA_BLACKSWORD.damage;
				stats.armor = CharacterLibrary.CHAR_AURA_BLACKSWORD.armor;
				info.name = "Aura Blacksword";
				info.age = 16;
				info.gender = CharacterInfo.CharacterGender.FEMALE;
				info.race = CharacterInfo.CharacterRace.HUMAN;
				break;
			}

			case CharacterType.HALLFRED_THORALDSON:
			{
				stats.health = CharacterLibrary.CHAR_HALLFRED_THORALDSON.health;
				stats.speed = CharacterLibrary.CHAR_HALLFRED_THORALDSON.speed;
				stats.damage = CharacterLibrary.CHAR_HALLFRED_THORALDSON.damage;
				stats.armor = CharacterLibrary.CHAR_HALLFRED_THORALDSON.armor;

				info.name = "Hallfred Thoraldson";
				info.age = 58;
				info.gender = CharacterInfo.CharacterGender.MALE;
				info.race = CharacterInfo.CharacterRace.DWARF;
				break;
			}

			case CharacterType.FREYA_SKAAR:
			{
				stats.health = CharacterLibrary.CHAR_FREYA_SKAAR.health;
				stats.speed = CharacterLibrary.CHAR_FREYA_SKAAR.speed;
				stats.damage = CharacterLibrary.CHAR_FREYA_SKAAR.damage;
				stats.armor = CharacterLibrary.CHAR_FREYA_SKAAR.armor;
				info.name = "Freya Skaar";
				info.age = 27;
				info.gender = CharacterInfo.CharacterGender.FEMALE;
				info.race = CharacterInfo.CharacterRace.DWARF;
				break;
			}
		}
	}

}

[System.Serializable]
public struct CharacterStats
{
	public float health;
	public float speed;
	public float damage;
	public float armor;
}

[System.Serializable]
public struct CharacterInfo
{
	public enum CharacterRace
	{
		HUMAN,
		DWARF,
	}

	public enum CharacterGender
	{
		MALE,
		FEMALE,
	}

	public string name;
	public CharacterRace race;
	public CharacterGender gender;
	public int age;
}

public class PlayerController : MonoBehaviour {

	void OnEnable()
	{
		EventManager.OnButtonClick += OnButtonClick;
	}

	void OnDisable()
	{
		EventManager.OnButtonClick -= OnButtonClick;
	}


	[Tooltip("Stats for current character")]
	public Character character;
	public bool withinEnemyRange;
	private Enemy contactingEnemy;
	void Start ()
	{
		SetCharacter(Character.CharacterType.FREYA_SKAAR);
	}

	void Update () {

	}


	public void SetCharacter(Character.CharacterType type)
	{
		character.type = type;
		character.Init();
	}


	void OnButtonClick(ButtonID id)
	{
		if (id == ButtonID.ATTACK)
		{
			if (withinEnemyRange && contactingEnemy != null)
			{
				contactingEnemy.TakeDamage(3);
			}
		}
	}
	public float Speed
	{
		get { return character.stats.speed;}
	}

	void OnCollisionStay(Collision col)
	{

		if (col.gameObject.tag == "Entity/Enemy")
		{
			withinEnemyRange = true;
			contactingEnemy = col.gameObject.transform.GetComponent<Enemy>();
		}

	}

	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Entity/Enemy")
		{
			withinEnemyRange = false;
			contactingEnemy = null;
		}
	}
}
