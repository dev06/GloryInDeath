using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class IngameUpgrade : MonoBehaviour
{
	public List<CharacterUpgrade> characterUpgrades;

	public UpgradeCard[] upgradeCards;

	private CanvasGroup _canvasGroup;
	void OnEnable()
	{
		EventManager.OnButtonClick += OnButtonClick;
		EventManager.OnGameEvent += OnGameEvent;
	}
	void OnDisable()
	{
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnGameEvent -= OnGameEvent;
	}

	void Start()
	{
		Toggle(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Display();
		}
	}

	void OnButtonClick(ButtonID id, SimpleButtonHandler handler)
	{
		if (id != ButtonID.UPG_HEALTH && id != ButtonID.UPG_DAMAGE && id != ButtonID.UPG_STAMINA) { return; }

		if (id == ButtonID.UPG_HEALTH)
		{
			CharacterAttributes a = new CharacterAttributes();
			a.SetAttributes(PlayerController.Instance.SessionAttributes);
			a.health += 50;
			PlayerController.Instance.Attributes.health += 25;
			PlayerController.Instance.SessionAttributes.SetAttributes(a);
		} else if (id == ButtonID.UPG_DAMAGE)
		{
			CharacterAttributes a = new CharacterAttributes();
			a.SetAttributes(PlayerController.Instance.SessionAttributes);
			a.Damage = a.Damage + (a.Damage * 0.15f);
			PlayerController.Instance.Attributes.SetAttributes("Damage", a);
			PlayerController.Instance.SessionAttributes.SetAttributes(a);
		} else if (id == ButtonID.UPG_STAMINA)
		{
			CharacterAttributes a = new CharacterAttributes();
			a.SetAttributes(PlayerController.Instance.SessionAttributes);
			a.stamina = a.stamina + (a.stamina * 0.1f);
			PlayerController.Instance.SessionAttributes.SetAttributes(a);
		}

		if (EventManager.OnGameEvent != null)
		{
			EventManager.OnGameEvent(EventID.CHARACTER_UPG);
		}

		Toggle(false);
	}

	void OnGameEvent(EventID id)
	{
		if (id == EventID.LEVEL_UP)
		{
			Display();
		}
	}
	public void Display()
	{
		Toggle(true);
		for (int i = 0; i < upgradeCards.Length; i++)
		{
			upgradeCards[i].Shuffle();
		}
	}


	private void Toggle(bool b)
	{
		if (_canvasGroup == null)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		_canvasGroup.alpha = b ? 1f : 0f;
		_canvasGroup.blocksRaycasts = b ;
	}

	private CharacterUpgrade ChooseUpgrade()
	{
		return characterUpgrades[Random.Range(0, characterUpgrades.Count)];
	}
}
