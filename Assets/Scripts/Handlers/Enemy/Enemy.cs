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
		this.health = attributes.health;
		this.speed = attributes.speed;
		this.damage = attributes.damage;
	}
}

public class Enemy: MonoBehaviour
{

	public EnemyAttributes defaultAttributes;

	[HideInInspector]
	private EnemyAttributes attributes;


	public Transform quad;
	private Transform healthBar;
	private ParticleSystem hurtParticles;

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
		healthBar = quad.transform.GetChild(1).transform;
		hurtParticles = transform.GetChild(2).transform.GetComponent<ParticleSystem>();
		attributes = new EnemyAttributes();
		attributes.SetAttributes(defaultAttributes);

	}

	public void Move()
	{
		ToggleSkin(true);
		attributes.SetAttributes(defaultAttributes);

		move = true;
	}
	void Update()
	{
		if (GameController.state != State.GAME) { return; }
		if (!move) { return; }
		agent.speed = attributes.speed;
		agent.SetDestination(player.transform.position);

		healthBar.localScale = Vector3.Lerp(healthBar.localScale, new Vector3((attributes.health * 2) / (defaultAttributes.health), .5f, .5f), Time.deltaTime * 10f);
	}


	public void TakeDamage(float damage)
	{
		attributes.health -= damage;
		if (isDead())
		{

			hurtParticles.Play();
			Reset();
			return;
		}

		hurtParticles.Play();



	}

	private void ToggleSkin(bool b)
	{
		GetComponent<MeshRenderer>().enabled = b;
	}

	public void Reset()
	{
		move = false;
		WaveController.Instance.ResetEnemyParent(this);
		transform.gameObject.SetActive(false);
	}

	public bool isDead()
	{
		attributes.health = Mathf.Clamp(attributes.health, 0f, attributes.health);

		bool dead = attributes.health <= 0;

		if (dead)
		{
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent(EventID.ENEMY_KILLED);
			}
		}
		return dead;
	}

	public bool IsMoving
	{
		get { return move; }
	}
}
