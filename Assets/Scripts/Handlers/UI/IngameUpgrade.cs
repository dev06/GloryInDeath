using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUpgrade : MonoBehaviour
{
	public List<CharacterUpgrade> characterUpgrades;
	public List<CharacterUpgrade> activeUpgrades;

	public UpgradeCard[] upgradeCards;

	private CanvasGroup _canvasGroup;
	void OnEnable ()
	{
		EventManager.OnButtonClick += OnButtonClick;
		EventManager.OnGameEvent += OnGameEvent;
	}
	void OnDisable ()
	{
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnGameEvent -= OnGameEvent;
	}

	void Start ()
	{
		Toggle (false);
		checkActiveUpgrades ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.P))
		{
			Display ();
		}
	}

	void checkActiveUpgrades ()
	{
		activeUpgrades.Clear ();
		activeUpgrades.Add (characterUpgrades[2]); //STAMINA
		activeUpgrades.Add (characterUpgrades[4]); //STAMINA REGEN

		bool _hasDamage = PlayerPrefs.HasKey ("UPG_DAMAGE") ? bool.Parse (PlayerPrefs.GetString ("UPG_DAMAGE")) : false;
		bool _hasCritHit = PlayerPrefs.HasKey ("UPG_CRITHIT") ? bool.Parse (PlayerPrefs.GetString ("UPG_CRITHIT")) : false;
		bool _hasHealth = PlayerPrefs.HasKey ("UPG_HEALTH") ? bool.Parse (PlayerPrefs.GetString ("UPG_HEALTH")) : false;

		if (_hasDamage) activeUpgrades.Add (characterUpgrades[1]);
		if (_hasCritHit) activeUpgrades.Add (characterUpgrades[3]);
		if (_hasHealth) activeUpgrades.Add (characterUpgrades[0]);

	}

	void OnButtonClick (ButtonID id, SimpleButtonHandler handler)
	{
		if (id != ButtonID.UPG_HEALTH && id != ButtonID.UPG_DAMAGE && id != ButtonID.UPG_STAMINA && id != ButtonID.UPG_CRITICAL && id != ButtonID.UPG_STAMINA_REGEN) { return; }

		if (id == ButtonID.UPG_HEALTH)
		{
			CharacterAttributes a = new CharacterAttributes ();
			a.SetAttributes (PlayerController.Instance.SessionAttributes);
			a.health += 50;
			PlayerController.Instance.Attributes.Health+=50; 
			PlayerController.Instance.SessionAttributes.SetAttributes (a);
		}
		else if (id == ButtonID.UPG_DAMAGE)
		{
			CharacterAttributes a = new CharacterAttributes ();
			a.SetAttributes (PlayerController.Instance.SessionAttributes);
			a.Damage = a.Damage + (a.Damage * 0.10f);
			PlayerController.Instance.Attributes.SetAttributes ("Damage", a);
			PlayerController.Instance.SessionAttributes.SetAttributes (a);
		}
		else if (id == ButtonID.UPG_STAMINA)
		{
			CharacterAttributes a = new CharacterAttributes ();
			a.SetAttributes (PlayerController.Instance.SessionAttributes);
			a.stamina = a.stamina + (a.stamina * 0.1f);
			PlayerController.Instance.SessionAttributes.SetAttributes (a);
		}
		else if (id == ButtonID.UPG_CRITICAL)
		{
			CharacterAttributes a = new CharacterAttributes ();
			a.SetAttributes (PlayerController.Instance.SessionAttributes);
			a.criticalHit = a.criticalHit + (a.criticalHit * .05f);
			PlayerController.Instance.Attributes.SetAttributes ("CriticalHit", a);
			PlayerController.Instance.SessionAttributes.SetAttributes (a);
		}
		else if (id == ButtonID.UPG_STAMINA_REGEN)
		{
			CharacterAttributes a = new CharacterAttributes ();
			a.SetAttributes (PlayerController.Instance.SessionAttributes);
			a.staminaRegen = a.staminaRegen + (a.staminaRegen * .05f);
			PlayerController.Instance.Attributes.SetAttributes ("StaminaRegen", a);
			PlayerController.Instance.SessionAttributes.SetAttributes (a);
		}

		if (EventManager.OnGameEvent != null)
		{
			EventManager.OnGameEvent (EventID.CHARACTER_UPG);
		}

		Toggle (false);
	}

	void OnGameEvent (EventID id)
	{
		switch (id)
		{
			case EventID.LEVEL_UP:
				{
					Display ();
					break;
				}

			case EventID.CHR_UPG_DAMAGE:
				{
					PlayerPrefs.SetString ("UPG_DAMAGE", true.ToString ());
					checkActiveUpgrades ();
					break;
				}
			case EventID.CHR_UPG_CRITHIT:
				{
					PlayerPrefs.SetString ("UPG_CRITHIT", true.ToString ());
					checkActiveUpgrades ();
					break;
				}
			case EventID.CHR_UPG_HEALTH:
				{
					PlayerPrefs.SetString ("UPG_HEALTH", true.ToString ());
					checkActiveUpgrades ();
					break;
				}

		}
	}
	public void Display ()
	{
		Toggle (true);
		for (int i = 0; i < upgradeCards.Length; i++)
		{
			upgradeCards[i].Shuffle ();
		}
	}

	private void Toggle (bool b)
	{
		if (_canvasGroup == null)
		{
			_canvasGroup = GetComponent<CanvasGroup> ();
		}

		_canvasGroup.alpha = b ? 1f : 0f;
		_canvasGroup.blocksRaycasts = b;
	}

	private CharacterUpgrade ChooseUpgrade ()
	{
		return activeUpgrades[Random.Range (0, activeUpgrades.Count)];
	}
}