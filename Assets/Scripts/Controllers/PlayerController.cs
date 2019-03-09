using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLibrary
{
	public static CharacterAttributes CHAR_AURA_BLACKSWORD = new CharacterAttributes(CharacterType.AURA_BLACKSWORD, 10, 5, 2, 3);
	public static CharacterAttributes CHAR_HALLFRED_THORALDSON = new CharacterAttributes(CharacterType.HALLFRED_THORALDSON, 5, 2, 3, 4);
	public static CharacterAttributes CHAR_FREYA_SKAAR = new CharacterAttributes(CharacterType.FREYA_SKAAR, 9, 5, 4, 5);

}

[System.Serializable]
public class CharacterAttributes
{
	public CharacterType type;
	public float health;
	public float speed;
	public float armor;
	public float damage;

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
	}

	public CharacterAttributes GetAttributes()
	{
		return this;
	}

}

public enum CharacterType
{
	AURA_BLACKSWORD,
	HALLFRED_THORALDSON,
	FREYA_SKAAR,
}


public class PlayerController : MonoBehaviour {


	public static PlayerController Instance;

	[HideInInspector]
	public CharacterAttributes attributes;
	public bool withinEnemyRange;
	private Enemy contactingEnemy;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void OnEnable()
	{
		EventManager.OnButtonClick += OnButtonClick;
	}

	void OnDisable()
	{
		EventManager.OnButtonClick -= OnButtonClick;
	}


	void Start ()
	{
	}

	void Update () {

	}


	public void SetCharacter(CharacterType type)
	{
		switch (type)
		{
			case CharacterType.AURA_BLACKSWORD:
			{

				attributes.SetAttributes(CharacterLibrary.CHAR_AURA_BLACKSWORD.GetAttributes());
				break;
			}
			case CharacterType.HALLFRED_THORALDSON:
			{

				attributes.SetAttributes(CharacterLibrary.CHAR_HALLFRED_THORALDSON.GetAttributes());
				break;
			}
			case CharacterType.FREYA_SKAAR:
			{

				attributes.SetAttributes(CharacterLibrary.CHAR_FREYA_SKAAR.GetAttributes());
				break;
			}

		}
	}


	void OnButtonClick(ButtonID id)
	{
		switch (id)
		{
			case ButtonID.ATTACK:
			{
				if (withinEnemyRange && contactingEnemy != null)
				{
					contactingEnemy.TakeDamage(3);
				}
				break;
			}

			case ButtonID.ABILITY:
			{
				if (withinEnemyRange && contactingEnemy != null)
				{
					contactingEnemy.TakeDamage(6);
				}
				break;
			}
		}
	}
	public float Speed
	{
		get { return attributes.speed;}
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
