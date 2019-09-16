using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DefaultCharacterAttribute
{
	public CharacterType type;
	public float health;
	public float speed;
	public float armor;
	public float damage;

	public float healthCost = 100;
	public float speedCost = 100;
	public float damageCost = 100;
	public float armorCost = 100;
}

public class CharacterModifiedLibrary
{
	public static CharacterAttributes CHAR_AURA_BLACKSWORD = new CharacterAttributes(CharacterType.AURA_BLACKSWORD, 100, 5, 2, 3);
	public static CharacterAttributes CHAR_HALLFRED_THORALDSON = new CharacterAttributes(CharacterType.HALLFRED_THORALDSON, 100, 1, 3, 4);
	public static CharacterAttributes CHAR_FREYA_SKAAR = new CharacterAttributes(CharacterType.FREYA_SKAAR, 100, 5, 4, 5);
}


public class PlayerController : MonoBehaviour {


	public static PlayerController Instance;

	public CharacterAttributes attributes;
	public CharacterAttributes sessionAttributes;
	public Transform activeCharacterTransform;
	public Transform[] characterSkins;
	public CharacterState characterState = CharacterState.IDLE;
	public bool withinEnemyRange;
	public bool walking;
	[HideInInspector]
	public bool lockMovement;

	private Enemy contactingEnemy;
	private Animator animator;
	private bool damageTaken, canAttack, canSpecialAttack;
	private Rigidbody rigidbody;
	private float damageTakenCoolDown;
	private float attackTimer;
	private float specialAttackTimer;
	private float autoAttackTimer;


	[Header("Particle System")]
	public ParticleSystem hammerSpecial;

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
		EventManager.OnGameEvent += OnGameEvent;
		EventManager.OnWeaponHit += OnWeaponHit;
		EventManager.OnSpecialAttackHit += OnSpecialAttackHit;
	}

	void OnDisable()
	{
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnGameEvent -= OnGameEvent;
		EventManager.OnWeaponHit -= OnWeaponHit;
		EventManager.OnSpecialAttackHit -= OnSpecialAttackHit;
	}


	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () {

		if (GameController.state != State.GAME) { return; }
		UpdateAnimations();
		UpdateAttackTimers();

		if (Input.GetKeyDown(KeyCode.Y))
		{
			StartCoroutine("IOnDeath");
		}

		// autoAttackTimer += Time.deltaTime;
		// if (autoAttackTimer > 1f)
		// {
		// 	Enemy e;
		// 	if (CheckWithinRange(2f, out e))
		// 	{
		// 		Quaternion lookRotation = Quaternion.LookRotation(e.transform.position - transform.position, Vector3.up);
		// 		lookRotation.x = lookRotation.z = 0;
		// 		transform.rotation = lookRotation;
		// 		if (canSpecialAttack)
		// 		{
		// 			TriggerAbility(e);
		// 		}
		// 		else
		// 		{
		// 			TriggerAttack(e);
		// 		}
		// 	}
		// }
	}

	void LateUpdate()
	{
		Vector3 position = transform.position;
		position.y = 0f;
		transform.position = position;
	}

	void OnGameEvent(EventID id)
	{
	}

	public bool isPlaying(string stateName)
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
		        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
			return true;
		}
		else {
			return false;
		}
	}

	public void UpdateAnimations()
	{
		animator.SetBool("Run", walking);
	}

	private void UpdateAttackTimers()
	{
		if (specialAttackTimer > 3f)
		{
			canSpecialAttack = true;
		}
		else
		{
			specialAttackTimer += Time.deltaTime;
		}
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

	public void OnWeaponHit(GameObject go)
	{
		if (go.tag != "Entity/Enemy") { return; }
		Enemy e;
		if (CheckWithinRange(2f, out e))
		{

			e.TakeDamage(Attributes.Damage);
			CameraController.Instance.TriggerShake(.2f);
		}
	}

	public void OnSpecialAttackHit(string _id)
	{
		float _distance = 0;
		switch (_id)
		{
			case "character_one":
			{
				_distance = 1f;
				break;
			}
			case "character_two":
			{
				_distance = 3f;
				CameraController.Instance.TriggerShake(.6f);
				hammerSpecial.Play();
				hammerSpecial.transform.position = activeCharacterTransform.position + new Vector3(0f, .3f, 0f);
				break;
			}
		}

		List<Enemy> e = GetEnemiesWithinRange(_distance);


		for (int i = 0; i < e.Count; i++)
		{
			e[i].TakeDamage(Attributes.Damage * 3f);
		}
	}

	public void SetCharacter(CharacterType type)
	{
		Attributes.SetAttributes(CharacterModel.SELECTED_MODEL.attributes);

		switch (type)
		{
			case CharacterType.AURA_BLACKSWORD:
			{
				ToggleSkin(0);

				break;
			}
			case CharacterType.HALLFRED_THORALDSON:
			{
				ToggleSkin(1);
				break;
			}
			case CharacterType.FREYA_SKAAR:
			{
				ToggleSkin(2);

				break;
			}
		}

		if (sessionAttributes == null)
		{
			sessionAttributes = new CharacterAttributes();
		}

		sessionAttributes.SetAttributes(Attributes);
	}



	void ToggleSkin(int i)
	{
		for (int j = 0; j < characterSkins.Length; j++)
		{
			characterSkins[j].gameObject.SetActive(false);
		}
		animator = characterSkins[i].GetComponent<Animator>();
		characterSkins[i].gameObject.SetActive(true);
		activeCharacterTransform = transform.GetChild(i + 1).gameObject.transform;
	}


	public void TriggerAttack()
	{
		animator.SetTrigger("AttackTrigger");
		attackTimer = 0;
		canAttack = false;
	}

	public void TriggerAttack(Enemy e)
	{
		//if (!canAttack) { return; }

		animator.SetTrigger("AttackTrigger");

		//e.TakeDamage(Attributes.Damage);
		CameraController.Instance.TriggerShake(.2f);

		attackTimer = 0;
		canAttack = false;
	}

	public void TriggerAbility()
	{
		if (!canSpecialAttack) { return; }
		animator.SetTrigger("SpecialTrigger");
		specialAttackTimer = 0;
		canSpecialAttack = false;
	}

	public void TriggerAbility(Enemy e)
	{
		if (!canSpecialAttack) { return; }
		animator.SetTrigger("SpecialTrigger");
		specialAttackTimer = 0;
		canSpecialAttack = false;
	}


	private bool CheckWithinRange(float threshold, out Enemy e)
	{
		bool b = false;
		e = null;
		for (int i = 0; i < WaveController.Instance.EnemyList.Count; i++)
		{
			if (!WaveController.Instance.EnemyList[i].gameObject.activeSelf) { continue; }

			float distance = (WaveController.Instance.EnemyList[i].transform.position - activeCharacterTransform.position).magnitude;

			if (distance < threshold)
			{
				b = true;
				e = WaveController.Instance.EnemyList[i];
				break;
			}
		}
		return b;
	}

	private List<Enemy> GetEnemiesWithinRange(float _range)
	{
		List<Enemy> _ret = new List<Enemy>();
		for (int i = 0; i < WaveController.Instance.EnemyList.Count; i++)
		{
			if (!WaveController.Instance.EnemyList[i].gameObject.activeSelf) { continue; }
			float distance = (WaveController.Instance.EnemyList[i].transform.position - activeCharacterTransform.position).magnitude;
			if (distance < _range)
			{
				_ret.Add(WaveController.Instance.EnemyList[i]);
			}
		}

		return _ret;
	}

	private float GetAttackDelay()
	{
		float ret =  2f - ( .05f * Attributes.Speed);
		ret = Mathf.Clamp(ret, .01f, ret);
		return ret;
	}


	void OnButtonClick(ButtonID id, SimpleButtonHandler handler)
	{

		switch (id)
		{
			case ButtonID.ATTACK:
			{
				Haptic.Vibrate(HapticIntensity.Light);
				TriggerAttack();
				autoAttackTimer = 0;
				handler.GetComponent<Animation>().Play();
				break;
			}

			case ButtonID.ABILITY:
			{
				Haptic.Vibrate(HapticIntensity.Medium);
				TriggerAbility();
				autoAttackTimer = 0;
				handler.GetComponent<Animation>().Play();
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

	void OnCollisionExit(Collision col)
	{
		if (col.gameObject.tag == "Entity/Enemy")
		{
			withinEnemyRange = false;
			contactingEnemy = null;
		}
	}

	public void TakeDamage(float damage)
	{
		float d = .05f * Attributes.Armor;
		damage = damage - d;
		damage = Mathf.Clamp(damage, .001f, damage);
		Attributes.health -= damage;

		if (Attributes.health <= 0)
		{
			LockMovement = true;
			if (!iOnDeathStarted)
			{
				StartCoroutine("IOnDeath");
				iOnDeathStarted = true;
			}
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
			return activeCharacterTransform.position;
		}
	}

	private bool iOnDeathStarted;
	IEnumerator IOnDeath()
	{
		var parameters = new Dictionary<string, object>();
		parameters["WAVE"] = WaveController.Instance.wave;
		FacebookManager.Instance.EventSent("Wave Failed", 1, parameters);
		GetComponentInChildren<MeshRenderer>().enabled = false;
		yield return new WaitForSeconds(1f);
		GameController.Instance.Reload();
	}

	public CharacterAttributes Attributes
	{
		get { return CharacterModel.SELECTED_MODEL.attributes; }
	}
	public float Speed
	{
		get { return Attributes.speed;}
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

			return Attributes.health / sessionAttributes.health;
		}
	}

	public string HealthText
	{
		get {
			return Mathf.Clamp((int)Attributes.Health, 0f, (int)Attributes.Health) + " / " + sessionAttributes.health;
		}
	}

	public float AttackCoolDown
	{
		get {
			float ret = (attackTimer / GetAttackDelay());
			ret = Mathf.Clamp(ret, 0f, 1f);
			return ret;
		}
	}

	public float SpecialCoolDown
	{
		get { return (specialAttackTimer / 3f); }
	}

	public float AutoAttackTimer
	{
		get { return autoAttackTimer; }
		set { this.autoAttackTimer = value; }
	}
}
