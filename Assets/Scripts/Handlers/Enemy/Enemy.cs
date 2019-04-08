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
	public bool walk;
	[HideInInspector]
	private EnemyAttributes attributes;
	public Transform quad;
	private Transform healthBar;
	private ParticleSystem hurtParticles, goldParticles;
	private PlayerController player;
	private NavMeshAgent agent;
	private bool move;
	private bool dead;
	private Animator animator;
	private Vector3 playerDestinationLocation;
	private Rigidbody rigidBody;
	void Start()
	{
		Init();
		player = FindObjectOfType<PlayerController>();

		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
	}

	public virtual void Init()
	{
		healthBar = quad.transform.GetChild(1).transform;
		hurtParticles = transform.GetChild(1).transform.GetComponent<ParticleSystem>();
		goldParticles = transform.GetChild(2).transform.GetComponent<ParticleSystem>();

		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
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

		dead = attributes.health <= 0;

		if (dead)
		{
			rigidBody.isKinematic = true;
			attributes.health = 0;
		}

		healthBar.localScale = Vector3.Lerp(healthBar.localScale, new Vector3((attributes.health * 2) / (defaultAttributes.health), .5f, .5f), Time.deltaTime * 10f);

		if (!move)
		{
			if (animator != null)
			{
				animator.SetBool("Run", false);

				animator.SetBool("Attack", false);
			}

			return;
		}




		agent.speed = attributes.speed;
		playerDestinationLocation = player.GetBodypoint;
		agent.SetDestination(playerDestinationLocation);
		Vector3 lookrotation = agent.steeringTarget - transform.position;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 6f * Time.deltaTime);

		if (animator != null)
		{
			animator.SetBool(walk ? "Walk" : "Run", agent.velocity.magnitude > 0);

			animator.SetBool("Attack", agent.remainingDistance < 2f);

		}
	}


	public void TakeDamage(float damage)
	{
		if (dead) return;

		attributes.health -= damage;

		if (isDead())
		{
			hurtParticles.Play();

			if (gameObject.activeSelf)
			{
				StopCoroutine("IReset");
				StartCoroutine("IReset");
			}

			return;
		}

		goldParticles.Play();

		hurtParticles.Play();
	}

	private void ToggleSkin(bool b)
	{
		GetComponentInChildren<MeshRenderer>().enabled = b;
		GetComponent<BoxCollider>().enabled = b;
	}

	public void Reset()
	{
		WaveController.Instance.ResetEnemyParent(this);
		transform.gameObject.SetActive(false);
	}

	IEnumerator IReset()
	{
		if (animator != null)
		{
			move = false;
			animator.SetTrigger("DeathTrigger");
		}

		yield return new WaitForSeconds(3f);

		Reset();
	}

	public EnemyAttributes Attributes
	{
		get {return attributes;}
	}

	public bool isDead()
	{
		attributes.health = Mathf.Clamp(attributes.health, 0f, attributes.health);

		bool dead = attributes.health <= 0;


		if (dead)
		{
			GetComponent<BoxCollider>().enabled = false;
			quad.gameObject.SetActive(false);

			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent(EventID.ENEMY_KILLED);
			}
		}
		return dead;
	}

	public bool IsDead
	{
		get {return dead;}
	}

	public bool IsMoving
	{
		get { return move; }
	}
}
