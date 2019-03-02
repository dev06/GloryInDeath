using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct EnemyWaveBuilder
{
	public EnemyType type;
	public float difficulty;
	public int quantity;
}
public enum EnemyType
{
	GOBLIN,
	ENT,
	ORC,
}

[System.Serializable]
public class EnemyAttributes
{
	public EnemyType type;
	public float health;
	public float speed;
	public float damage;
	public float difficulty;

	public EnemyAttributes()
	{

	}

	public EnemyAttributes(EnemyType type, float health, float speed, float damage, float difficulty)
	{

		this.difficulty = difficulty;
		this.type = type;
		this.health = health * difficulty;
		this.speed = speed;
		this.damage = damage * difficulty;
	}

	public void SetAttributes(EnemyAttributes attributes)
	{

		this.difficulty = attributes.difficulty;
		this.type = attributes.type;
		this.health = attributes.health * difficulty;
		this.speed = attributes.speed;
		this.damage = attributes.damage * difficulty;
	}
}

public class Enemy: MonoBehaviour
{

	public EnemyAttributes defaultAttributes;

	[HideInInspector]
	private EnemyAttributes attributes;


	public Transform quad;

	private PlayerController player;

	private NavMeshAgent agent;

	private bool move;
	void Start()
	{
		Init();
		player = FindObjectOfType<PlayerController>();
		agent = GetComponent<NavMeshAgent>();
	}

	public virtual void Init()
	{
		//defaultAttributes = new EnemyAttributes(type, health, speed, damage, difficulty);
		attributes = new EnemyAttributes();
		attributes.SetAttributes(defaultAttributes);

	}

	public void Move()
	{
		attributes.SetAttributes(defaultAttributes);
		UpdateHealthBar();
		move = true;
	}
	void Update()
	{
		if (GameController.state != State.GAME) return;
		if (!move) return;
		agent.speed = attributes.speed;
		agent.SetDestination(player.transform.position);

		// quad.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position, Vector3.up);
		// quad.transform.LookAt(Camera.main.transform.position);
		// quad.transform.rotation *= Quaternion.Euler(0, 180, 0);

	}


	public void TakeDamage(float damage)
	{
		attributes.health -= damage;

		UpdateHealthBar();

		if (isDead())
		{
			Reset();
		}
	}

	private void UpdateHealthBar()
	{
		quad.transform.GetChild(1).transform.localScale = new Vector3((attributes.health * 2) / (defaultAttributes.health), .5f, .5f);
	}

	public void Reset()
	{
		move = false;
		transform.gameObject.SetActive(false);
	}

	public bool isDead()
	{
		attributes.health = Mathf.Clamp(attributes.health, 0f, attributes.health);
		return attributes.health <= 0;
	}
}
