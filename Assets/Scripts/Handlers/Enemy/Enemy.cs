using System.Collections;
using System.Collections.Generic;
using TMPro;
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
	public float damage;
	public float maxHealth;

	public EnemyAttributes ()
	{

	}

	public void SetAttributes (EnemyAttributes attributes)
	{
		this.type = attributes.type;
		this.health = attributes.health;
		this.maxHealth = attributes.maxHealth;
		this.damage = attributes.damage;
	}

	public void SetAttributes (EnemyStats stats)
	{
		this.maxHealth = stats.health;
		this.health = stats.health;
		this.damage = stats.damage;
		this.type = stats.type;
	}
}

public class Enemy : MonoBehaviour
{
	public EnemyType type;
	public EnemyAttributes sessionAttributes;
	public bool walk;
	public HealthHandler healthHandler;
	public ParticleSystem fireFX;

	[HideInInspector]
	private EnemyAttributes attributes, waveAttributes;
	private PlayerController player;
	private ParticleSystem hurtParticles, goldParticles;
	private NavMeshAgent agent;
	private Animator animator;
	private Vector3 playerDestinationLocation;
	private Rigidbody rigidBody;
	private Animator _animator;
	private Transform _healthContainer;
	private Transform _cameraTransform;
	private bool move, dead, canAttack;
	private bool _stun;
	protected bool _inFireArea, _fireActive;
	private float attackTimer, _stunTimer;
	private float _fireActiveTimer;

	void OnEnable ()
	{
		EventManager.OnGameEvent += OnGameEvent;
	}
	void OnDisable ()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	void Start ()
	{
		Init ();
		player = FindObjectOfType<PlayerController> ();
		_animator = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.updatePosition = false;
	}

	public virtual void Init ()
	{
		hurtParticles = transform.GetChild (1).transform.GetComponent<ParticleSystem> ();
		goldParticles = transform.GetChild (2).transform.GetComponent<ParticleSystem> ();
		animator = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
		// sessionAttributes = new EnemyAttributes();
		// sessionAttributes.SetAttributes(enemyStats);
		_healthContainer = transform.GetChild (0);
		_cameraTransform = Camera.main.transform;
	}

	public void Move ()
	{
		ToggleSkin (true);
		gameObject.tag = "Entity/Enemy";
		sessionAttributes = getAttributes ();

		attributes = new EnemyAttributes ();
		attributes.SetAttributes (sessionAttributes);

		updateHealthDisplay ();
		healthHandler.Toggle (true);
		move = true;
	}

	void Update ()
	{
		if (GameController.state != State.GAME) { return; }
		rigidBody.velocity = Vector3.zero;
		if (!move)
		{
			if (animator != null)
			{
				animator.SetBool ("Run", false);
				animator.SetBool ("Attack", false);
			}

			return;
		}

		updateTimers ();
		updateAnimations ();
		updateRotations ();

		dead = attributes.health <= 0;
		agent.updatePosition = agent.updateRotation = !dead;
	}

	private void updateTimers ()
	{
		if (Stun)
		{
			_stunTimer += Time.unscaledDeltaTime;
			if (_stunTimer > 1f)
			{
				_stunTimer = 0f;
				Stun = false;
			}
		}

		if (_inFireArea)
		{
			_fireActiveTimer = 0f;
		}

		if (_fireActive)
		{
			takeSimpleDamage (1f * Time.deltaTime);
			_fireActiveTimer += Time.deltaTime;
			if (_fireActiveTimer > 2f)
			{
				toggleFire (false);
			}
		}
	}

	private void toggleFire (bool b)
	{
		if (fireFX == null) { return; }
		if (b)
		{
			fireFX.Play ();
			_fireActive = true;
		}
		else
		{
			fireFX.Stop ();
			_fireActive = false;
			_fireActiveTimer = 0f;

		}
	}

	private void updateAnimations ()
	{
		_healthContainer.LookAt (_cameraTransform.position);
		Vector3 _deltaPos = player.GetBodypoint - transform.position;
		float _distance = _deltaPos.magnitude;
		bool _canAttack = _distance < 2f && !Stun;
		if (PlayerController.Instance.IsDead)
		{
			animator.SetBool ("Idle", true);
		}
		else
		{
			animator.SetBool ("Attack", _canAttack);
			if (!Stun)
			{
				animator.SetBool (walk ? "Walk" : "Run", !_canAttack);
			}
			else
			{
				animator.SetBool (walk ? "Walk" : "Run", false);

			}
		}
	}

	private void updateRotations ()
	{
		Vector3 lookrotation = player.GetBodypoint - transform.position;
		if (lookrotation == Vector3.zero || dead) { return; }
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookrotation), 6f * Time.deltaTime);
	}

	void OnAnimatorMove ()
	{
		if (GameController.PAUSE || _animator == null || agent == null || dead) { return; }
		Vector3 v = _animator.deltaPosition / Time.deltaTime;

		if (!float.IsNaN (v.x) && !float.IsNaN (v.y) && !float.IsNaN (v.z))
		{
			agent.velocity = v;
		}
	}

	public void OnGameEvent (EventID _id)
	{
		if (_id == EventID.LEVEL_UP)
		{
			// EnemyAttributes a = new EnemyAttributes();
			// a.health = sessionAttributes.health + (sessionAttributes.health * .1f);
			// a.damage = sessionAttributes.damage + (sessionAttributes.damage * .1f);
			// sessionAttributes.SetAttributes(a);
		}
	}

	public void Attack ()
	{
		PlayerController.Instance.TakeDamage (attributes.damage);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Env/Fire")
		{
			_inFireArea = true;
			toggleFire (true);
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "Env/Fire")
		{
			_inFireArea = false;
		}
	}

	private void takeSimpleDamage (float damage)
	{
		attributes.health -= damage;
		GameController.Instance.gameOverStatsSO.damageDealt += damage;
		attributes.health = Mathf.Clamp (attributes.health, 0f, sessionAttributes.health);
		updateHealthDisplay ();
		if (isDead ())
		{

			healthHandler.Toggle (false);
			hurtParticles.Play ();
			if (gameObject.activeSelf)
			{
				StartCoroutine ("IReset");
			}
			return;
		}
	}

	public void TakeDamage (float damage, bool _shouldStun = true)
	{
		if (dead) { return; }
		Stun = _shouldStun;
		animator.SetTrigger ("Hit");
		attributes.health -= damage;
		if (Random.value < .5f)
		{
			SoundManager.Instance.PlaySFX (SoundManager.Hammer_Hit_Body_Gore, Random.Range (.8f, 1.2f));
		}
		else
		{
			SoundManager.Instance.PlaySFX (SoundManager.Hammer_Hit_Metal_Armor, Random.Range (.8f, 1.2f));

		}
		GameController.Instance.gameOverStatsSO.damageDealt += damage;
		attributes.health = Mathf.Clamp (attributes.health, 0f, sessionAttributes.health);
		updateHealthDisplay ();
		if (isDead ())
		{

			healthHandler.Toggle (false);
			hurtParticles.Play ();
			if (gameObject.activeSelf)
			{
				StartCoroutine ("IReset");
			}
			return;
		}
		else
		{
			goldParticles.Play ();
			hurtParticles.Play ();
		}
	}

	private EnemyAttributes getAttributes ()
	{
		switch (type)
		{
			case EnemyType.GOBLIN:
				{
					return EnemyController.Instance.goblinAttributes;
				}
			case EnemyType.ORC:
				{
					return EnemyController.Instance.orcAttributes;
				}
		}
		return null;
	}

	private void updateHealthDisplay ()
	{
		healthHandler.health = (Attributes.health / sessionAttributes.health);
		healthHandler.healthString = ((int) Attributes.health + "/" + (int) sessionAttributes.health);
	}

	private void ToggleSkin (bool b)
	{
		GetComponentInChildren<MeshRenderer> ().enabled = b;
		GetComponent<BoxCollider> ().enabled = b;
	}

	public void Reset ()
	{
		WaveController.Instance.ResetEnemyParent (this);
	}

	IEnumerator IReset ()
	{
		if (animator != null)
		{
			move = false;
			animator.SetTrigger ("DeathTrigger");
		}

		yield return new WaitForSeconds (3f);

		Reset ();
	}

	public EnemyAttributes Attributes
	{
		get { return attributes; }
	}

	public bool isDead ()
	{
		attributes.health = Mathf.Clamp (attributes.health, 0f, attributes.health);
		bool dead = (int) attributes.health <= 0;
		if (dead)
		{
			if (Random.value < DropProbs.HEALTH_POTION)
			{
				DropHandler.Instance.Drop (transform.position + new Vector3 (Random.Range (-1f, 1f), 3f, Random.Range (-1f, 1f)));
			}

			GetComponent<BoxCollider> ().enabled = false;
			rigidBody.isKinematic = true;
			GameController.Instance.gameOverStatsSO.kills++;
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent (EventID.ENEMY_KILLED);
			}
		}
		return dead;
	}

	public bool IsDead
	{
		get { return dead; }
	}

	public bool IsMoving
	{
		get { return move; }
	}

	public bool Stun
	{
		get { return _stun; }
		set { this._stun = value; }
	}

}