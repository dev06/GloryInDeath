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



public struct StaminaCost
{
	public static int SPECIAL_ATK_COST = 30;
	public static int REGULAR_ATK_COST = 20;
	public static int ROLL_COST = 10;
}

public struct LevelInfo
{
	public int level;
	public int kills;
	public int killsRequired;

	public void Init()
	{
		level = 1;
		killsRequired = 5;
		kills = 0;
	}

	public void LevelUp()
	{
		level++;
		killsRequired += 3;
		kills = 0;
		if (EventManager.OnGameEvent != null)
		{
			EventManager.OnGameEvent(EventID.LEVEL_UP);
		}
	}

	public void UpdateData()
	{
		kills++;
		if (Ratio >= 1)
		{
			LevelUp();
		}
	}

	public float Ratio
	{
		get { return (float)(kills) / (float)(killsRequired); }
	}
}


public class PlayerController : MonoBehaviour {


	public static PlayerController Instance;

	public DisplayText displayText;
	public LevelInfo levelInfo;
	public CharacterAttributes sessionAttributes;
	public Transform activeCharacterTransform;
	public Transform[] characterSkins;
	public CharacterState characterState = CharacterState.IDLE;
	public bool withinEnemyRange;
	public bool walking;
	[HideInInspector]
	public bool lockMovement;

	private float _staminaRecoverRate = 10f;
	private Enemy contactingEnemy;
	private Animator animator;
	private bool damageTaken, canAttack, canSpecialAttack, isDead;
	private Rigidbody rigidbody;
	private float damageTakenCoolDown;
	private float attackTimer;
	private float specialAttackTimer;
	private float autoAttackTimer;
	private float _abilityDoublePressTimer;
	private float _threshold = .5f;
	private bool _inAttackRange;
	private bool _abilityPressed;


	[Header("Particle System")]
	public ParticleSystem hammerSpecial;
	public ParticleSystem upg_particles;
	public ParticleSystem healthPotionParticles;





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
		EventManager.OnRegularAttackStart += OnRegularAttackStart;
		EventManager.OnRegularAttackEnd += OnRegularAttackEnd;
	}

	void OnDisable()
	{
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnGameEvent -= OnGameEvent;
		EventManager.OnWeaponHit -= OnWeaponHit;
		EventManager.OnSpecialAttackHit -= OnSpecialAttackHit;
		EventManager.OnRegularAttackStart -= OnRegularAttackStart;
		EventManager.OnRegularAttackEnd -= OnRegularAttackEnd;
	}


	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		levelInfo.Init();
	}

	void Update () {

		if (GameController.state != State.GAME) { return; }
		UpdateAnimations();
		UpdateAttackTimers();

		if (Input.GetKeyDown(KeyCode.H))
		{
			StartCoroutine("IPlayUpgParticles");
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			levelInfo.LevelUp();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			levelInfo.UpdateData();
		}

	}

	void LateUpdate()
	{
		Vector3 position = transform.position;
		position.y = 0f;
		transform.position = position;
	}

	[Range(.05f, 1f)]
	public float length = .5f;
	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.WAVE_END:
			{
				LockMovement = true;
				animator.SetBool("Victory", true);
				break;
			}
			case EventID.ENEMY_KILLED:
			{
				levelInfo.UpdateData();
				break;
			}
			case EventID.CHARACTER_UPG:
			{
				if (upg_particles != null)
				{
					IEnumerator _i = IPlayUpgParticles();
					StartCoroutine(_i);
				}
				break;
			}
		}
	}

	IEnumerator IPlayUpgParticles()
	{
		upg_particles.Play();
		yield return new WaitForSecondsRealtime(length);
		upg_particles.Stop();
		StopCoroutine("IPlayUpgParticles");
	}

	IEnumerator IPlayHealthPotionParticles()
	{
		healthPotionParticles.Play();
		yield return new WaitForSecondsRealtime(.2f);
		healthPotionParticles.Stop();
		StopCoroutine("IPlayHealthPotionParticles");
	}

	void OnRegularAttackStart(string _id)
	{
		_inAttackRange = true;
		Stamina -= StaminaCost.REGULAR_ATK_COST;

	}
	void OnRegularAttackEnd(string _id)
	{
		_inAttackRange = false;
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
		if (isDead)
		{
			animator.SetBool("Run", false);
			return;
		}
		animator.SetBool("Run", walking);
	}

	private void UpdateAttackTimers()
	{
		canSpecialAttack = Attributes.stamina >= StaminaCost.SPECIAL_ATK_COST;
		Attributes.stamina += Time.deltaTime * _staminaRecoverRate;
		Attributes.stamina = Mathf.Clamp(Attributes.stamina, 0f, sessionAttributes.stamina);

		if (_abilityPressed)
		{
			_abilityDoublePressTimer += Time.deltaTime;
			if (_abilityDoublePressTimer > _threshold)
			{
				_abilityDoublePressTimer = 0;
				_abilityPressed = false;
			}
		}
	}

	public void ToggleRunning(bool b)
	{
		animator.SetBool("Run", b);
	}

	public void SetState(CharacterState state)
	{
		characterState = state;
	}

	public void OnWeaponHit(GameObject go)
	{
		if (go.tag != "Entity/Enemy" || !_inAttackRange)
		{
			return;
		}

		List<Enemy> e = GetEnemiesWithinRange(2);

		for (int i = 0; i < e.Count; i++)
		{
			e[i].TakeDamage(Attributes.Damage);
			displayText.Show("-" + Attributes.Damage.ToString("F1"), Color.red);
			e[i].Stun = true;
		}
		CameraController.Instance.TriggerShake(.2f);
	}

	public void OnSpecialAttackHit(string _id)
	{
		float _distance = 0;
		Stamina -= 50f;
		switch (_id)
		{
			case "character_one":
			{
				_distance = 3.5f;
				break;
			}
			case "character_two":
			{
				_distance = 4f;
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
			displayText.Show("-" + (Attributes.Damage * 3f).ToString("F1"), Color.red);
			e[i].Stun = true;
		}
	}

	public void SetCharacter(CharacterType type)
	{
		Attributes.SetAttributes(CharacterModel.SELECTED_MODEL.stats);

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
		animator.SetInteger("attackIndex", Random.Range(0, 4));
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

	private bool CheckWithinRange(float threshold, out Enemy e)
	{
		bool b = false;
		e = null;
		Transform _waveT = WaveController.Instance.EnemyWaveTransform;
		for (int i = 0; i < _waveT.childCount; i++)
		{
			Enemy ee = _waveT.GetChild(i).GetComponent<Enemy>();
			if (ee.Attributes.health <= 0) { continue; }
			float distance = (ee.transform.position - activeCharacterTransform.position).magnitude;

			if (distance < threshold)
			{
				b = true;
				e = ee;
				break;
			}
		}
		return b;
	}

	private List<Enemy> GetEnemiesWithinRange(float _range)
	{
		List<Enemy> _ret = new List<Enemy>();
		Transform _waveT = WaveController.Instance.EnemyWaveTransform;
		for (int i = 0; i < _waveT.childCount; i++)
		{
			Enemy ee = _waveT.GetChild(i).GetComponent<Enemy>();
			if (!ee.gameObject.activeSelf) { continue; }
			float distance = (ee.transform.position - activeCharacterTransform.position).magnitude;
			if (distance < _range)
			{
				_ret.Add(ee);
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
		if (IsDead) { return; }

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

			case ButtonID.DEFENSE:
			{
				if (!isPlaying("BaseLayer.Roll") && Stamina >= StaminaCost.ROLL_COST)
				{
					animator.SetTrigger("Roll");
					Stamina  -= StaminaCost.ROLL_COST;
					Haptic.Vibrate(HapticIntensity.Medium);
					handler.GetComponent<Animation>().Play();
				}
				break;
			}

			case ButtonID.ABILITY:
			{

				// if (_abilityPressed && _abilityDoublePressTimer < _threshold && Stamina >= StaminaCost.ROLL_COST)
				// {

				// } else
				// {
				// }
				TriggerAbility();

				_abilityPressed = true;
				Haptic.Vibrate(HapticIntensity.Medium);
				handler.GetComponent<Animation>().Play();
				break;
			}
		}
	}

	public Gradient g;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Entity/Enemy")
		{
			contactingEnemy = col.gameObject.transform.GetComponent<Enemy>();
		}
		if (col.gameObject.tag == "Objects/HealthPotion")
		{
			if (healthPotionParticles != null)
			{
				IEnumerator _i = IPlayHealthPotionParticles();
				StartCoroutine(_i);
			}
			float _percentToAdd = .1f;
			col.gameObject.GetComponent<HealthPotion>().PickUp();
			Attributes.health = Attributes.health + (sessionAttributes.health * _percentToAdd);
			Attributes.health = Mathf.Clamp(Attributes.health, 0f, sessionAttributes.health);
			displayText.Show("+" +  (sessionAttributes.health * _percentToAdd).ToString("F0"), Color.green);
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
		if (isDead) { return; }
		damage = Mathf.Clamp(damage, .001f, damage);
		Attributes.health -= damage;
		GameController.Instance.gameOverStatsSO.damageTaken += damage;

		if (Attributes.health <= 0)
		{
			LockMovement = true;
			animator.SetTrigger("DeathTrigger");
			isDead = true;
			GetComponent<Rigidbody>().isKinematic = true;
			activeCharacterTransform.GetComponent<Collider>().enabled = false;
			if (!iOnDeathStarted)
			{
				var parameters = new Dictionary<string, object>();
				parameters["WAVE"] = WaveController.Instance.wave;
				//FacebookManager.Instance.EventSent("Wave Failed", 1, parameters);
				//GetComponentInChildren<MeshRenderer>().enabled = false;
				iOnDeathStarted = true;

				if (EventManager.OnGameEvent != null)
				{
					EventManager.OnGameEvent(EventID.DEATH);
				}
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

	public string LevelString
	{
		get {return "Lvl " + levelInfo.level; }
	}

	public float LevelProgress
	{
		get {return levelInfo.Ratio;}
	}

	public CharacterAttributes SessionAttributes
	{
		get { return sessionAttributes; }
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

	public float StaminaRatio
	{
		get {return Attributes.stamina / sessionAttributes.stamina;}
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

	public bool IsDead
	{
		get {return isDead; }
	}

	public float AutoAttackTimer
	{
		get { return autoAttackTimer; }
		set { this.autoAttackTimer = value; }
	}

	public float Stamina
	{
		get {return Attributes.stamina; }
		set {this.Attributes.stamina = value; }
	}
}
