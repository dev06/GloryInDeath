﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLibrary
{
	public static CharacterAttributes CHAR_AURA_BLACKSWORD = new CharacterAttributes(CharacterType.AURA_BLACKSWORD, 10, 2, 2, 3);
	public static CharacterAttributes CHAR_HALLFRED_THORALDSON = new CharacterAttributes(CharacterType.HALLFRED_THORALDSON, 2, 2f, 3, 4);
	public static CharacterAttributes CHAR_FREYA_SKAAR = new CharacterAttributes(CharacterType.FREYA_SKAAR, 9, 2, 4, 5);

}

public class CharacterModifiedLibrary
{
	public static CharacterAttributes CHAR_AURA_BLACKSWORD = new CharacterAttributes(CharacterType.AURA_BLACKSWORD, 10, 5, 2, 3);
	public static CharacterAttributes CHAR_HALLFRED_THORALDSON = new CharacterAttributes(CharacterType.HALLFRED_THORALDSON, 5, 2f, 3, 4);
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
	}

	public CharacterAttributes GetAttributes()
	{
		return this;
	}

	public float Health
	{
		get {return health;}
		set {
			this.health = value;
			this.health = Mathf.Clamp(health, 1f, 30f);
		}
	}
	public float Speed
	{
		get {return speed;}
		set {
			this.speed = value;
			this.speed = Mathf.Clamp(speed, 1f, 30f);
		}
	}
	public float Damage
	{
		get {return damage;}
		set {
			this.damage = value;
			this.damage = Mathf.Clamp(damage, 1f, 30f);
		}
	}
	public float Armor
	{
		get {return armor;}
		set {
			this.armor = value;
			this.armor = Mathf.Clamp(armor, 1f, 30f);
		}
	}


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

public class PlayerController : MonoBehaviour {


	public static PlayerController Instance;

	[HideInInspector]
	public CharacterAttributes attributes;
	public bool withinEnemyRange;
	private Enemy contactingEnemy;
	private Animator animator;

	[HideInInspector]
	public CharacterAttributes defaultAttriubtes;
	[HideInInspector]
	public bool lockMovement;

	public bool walking;

	private bool damageTaken;
	private float damageTakenCoolDown;
	private Rigidbody rigidbody;
	public CharacterState characterState = CharacterState.IDLE;
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
		defaultAttriubtes = new CharacterAttributes();
		rigidbody = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	void Update () {

		if (GameController.state != State.GAME) { return; }

		if (damageTaken)
		{
			damageTakenCoolDown += Time.deltaTime;

			if (damageTakenCoolDown > 1.5f)
			{
				damageTaken = false;
				damageTakenCoolDown = 0;
			}
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			SetState(CharacterState.ATTACK);
		}

		UpdateAnimations();


	}

	public bool isPlaying(string stateName)
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
		        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
			return true;
		else
			return false;
	}

	public void UpdateAnimations()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			animator.SetTrigger("AttackTrigger");
		}

		animator.SetBool("Run", walking);
	}

	public void ToggleRunning(bool b)
	{

		animator.SetBool("Run", b);

	}

	public void SetState(CharacterState state)
	{
		characterState = state;

		ToggleRunning(state == CharacterState.RUN);
	}


	public void SetCharacter(CharacterType type)
	{
		switch (type)
		{
			case CharacterType.AURA_BLACKSWORD:
			{

				attributes.SetAttributes(CharacterModifiedLibrary.CHAR_AURA_BLACKSWORD.GetAttributes());
				break;
			}
			case CharacterType.HALLFRED_THORALDSON:
			{

				attributes.SetAttributes(CharacterModifiedLibrary.CHAR_HALLFRED_THORALDSON.GetAttributes());
				break;
			}
			case CharacterType.FREYA_SKAAR:
			{

				attributes.SetAttributes(CharacterModifiedLibrary.CHAR_FREYA_SKAAR.GetAttributes());
				break;
			}

		}

		if (defaultAttriubtes == null)
		{
			defaultAttriubtes = new CharacterAttributes();
		}

		defaultAttriubtes.SetAttributes(attributes);
	}


	void OnButtonClick(ButtonID id)
	{
		switch (id)
		{
			case ButtonID.ATTACK:
			{
				if (withinEnemyRange && contactingEnemy != null)
				{
					contactingEnemy.TakeDamage(attributes.Damage);
					if (contactingEnemy.Attributes.health <= 0)
					{
						contactingEnemy = null;
					}
					CameraController.Instance.TriggerShake(.2f);
				}
				animator.SetTrigger("AttackTrigger");
				break;
			}

			case ButtonID.ABILITY:
			{
				if (withinEnemyRange && contactingEnemy != null)
				{
					contactingEnemy.TakeDamage(attributes.Damage * 2f);
					CameraController.Instance.TriggerShake(.2f);
					if (contactingEnemy.Attributes.health <= 0)
					{
						contactingEnemy = null;
					}
				}

				animator.SetTrigger("AttackTrigger");

				break;
			}
		}
	}


	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Entity/Enemy")
		{
			contactingEnemy = col.gameObject.transform.GetComponent<Enemy>();

		}
	}

	public void TakeDamage(float damage)
	{
		attributes.health -= damage;

		if (attributes.health <= 0)
		{
			LockMovement = true;
			StopCoroutine("IOnDeath");
			StartCoroutine("IOnDeath");
		}

		if (EventManager.OnGameEvent != null)
		{
			EventManager.OnGameEvent(EventID.PLAYER_HURT);
		}
	}

	public Vector3 GetBodypoint
	{
		get
		{
			Transform bodyPoints = transform.GetChild(1);
			Vector3 point = bodyPoints.GetChild(Random.Range(0, bodyPoints.childCount)).transform.position;
			return point;
		}
	}


	IEnumerator IOnDeath()
	{
		GetComponentInChildren<MeshRenderer>().enabled = false;
		yield return new WaitForSeconds(1f);
		GameController.Instance.Reload();
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

			if (!damageTaken)
			{
				TakeDamage(.5f);
				damageTaken = true;
			}
		}


	}

	public bool LockMovement
	{
		get {
			return lockMovement;
		}

		set { this.lockMovement = value; }
	}

	public float HealthRatio
	{
		get {

			return attributes.health / defaultAttriubtes.health;
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
