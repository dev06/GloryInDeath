using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
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
		this.health = attributes.health ;
		this.speed = attributes.speed;
		this.damage = attributes.damage;
	}
}

public class Enemy: MonoBehaviour
{

	public EnemyAttributes defaultAttributes;
	public bool walk;
	public HealthHandler healthHandler;

	[HideInInspector]
	private EnemyAttributes attributes, waveAttributes;
	private ParticleSystem hurtParticles, goldParticles;
	private PlayerController player;
	private NavMeshAgent agent;
	private bool move, dead, canAttack;
	private Animator animator;
	private Vector3 playerDestinationLocation;
	private Rigidbody rigidBody;
	private float attackTimer, _stunTimer;
	private Transform _healthContainer;
	private Animator _animator;
	private Transform _cameraTransform;
	private bool _stun;


	void OnEnable()
	{
		EventManager.OnRegularAttack += OnRegularAttack;
	}
	void OnDisable()
	{
		EventManager.OnRegularAttack -= OnRegularAttack;
	}

	void Start()
	{
		Init();
		player = FindObjectOfType<PlayerController>();
		_animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
	}

	public virtual void Init()
	{
		hurtParticles = transform.GetChild(1).transform.GetComponent<ParticleSystem>();
		goldParticles = transform.GetChild(2).transform.GetComponent<ParticleSystem>();
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		attributes = new EnemyAttributes();
		attributes.SetAttributes(defaultAttributes);
		_healthContainer = transform.GetChild(0);
		_cameraTransform = Camera.main.transform;
	}

	public void Move()
	{
		ToggleSkin(true);
		waveAttributes = new EnemyAttributes();
		waveAttributes.SetAttributes(defaultAttributes);
		waveAttributes.health = defaultAttributes.health * (float)(WaveController.Instance.wave) * .45f;
		waveAttributes.damage = defaultAttributes.damage * (float)(WaveController.Instance.wave) * .3f;

		defaultAttributes.SetAttributes(waveAttributes);
		attributes.SetAttributes(defaultAttributes);

		healthHandler.health = (Attributes.health / defaultAttributes.health);
		healthHandler.healthString = (Attributes.health + "/" + defaultAttributes.health);
		move = true;
	}
	void Update()
	{
		if (GameController.state != State.GAME) { return; }
		rigidBody.velocity = Vector3.zero;

		if (!move)
		{
			if (animator != null)
			{
				animator.SetBool("Run", false);
				animator.SetBool("Attack", false);
			}

			return;
		}

		if (Stun)
		{
			_stunTimer += Time.unscaledDeltaTime;
			if (_stunTimer > 1f)
			{
				_stunTimer = 0f;
				Stun = false;
			}
		}

		_healthContainer.LookAt(_cameraTransform.position);
		Vector3 _deltaPos = player.GetBodypoint - transform.position;
		float _distance = _deltaPos.magnitude;
		bool _canAttack = _distance < 2f && !Stun;
		if (PlayerController.Instance.IsDead)
		{
			animator.SetBool("Idle", true);
		} else
		{
			animator.SetBool("Attack", _canAttack);
			if (!Stun)
			{
				animator.SetBool(walk ? "Walk" : "Run", !_canAttack);
			} else
			{
				animator.SetBool(walk ? "Walk" : "Run", false);

			}
		}

		Vector3 lookrotation = player.GetBodypoint - transform.position;
		if (lookrotation != Vector3.zero && !dead)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), 6f * Time.deltaTime);
		}

		dead = attributes.health <= 0;
		agent.updatePosition = agent.updateRotation = !dead;
	}

	void OnAnimatorMove()
	{
		if (GameController.PAUSE || _animator == null || agent == null || dead) { return; }
		Vector3 v = _animator.deltaPosition / Time.deltaTime;
		if (!float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z))
		{
			agent.velocity = v;
		}
	}

	public void OnRegularAttack(string _id)
	{
	}

	public void Attack()
	{
		PlayerController.Instance.TakeDamage(attributes.damage);
	}


	public void TakeDamage(float damage)
	{
		if (dead) { return; }

		animator.SetTrigger("Hit");
		attributes.health -= damage;
		attributes.health = Mathf.Clamp(attributes.health, 0f , attributes.health);
		healthHandler.health = (Attributes.health / defaultAttributes.health);
		healthHandler.healthString = ((int)Attributes.health + "/" + (int)defaultAttributes.health);
		if (isDead())
		{
			healthHandler.Toggle(false);
			hurtParticles.Play();
			if (gameObject.activeSelf)
			{
				StartCoroutine("IReset");
			}
			return;
		} else
		{
			goldParticles.Play();
			hurtParticles.Play();
		}

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
			rigidBody.isKinematic = true;
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

	public bool Stun
	{
		get {return _stun; }
		set {this._stun = value; }
	}
}
