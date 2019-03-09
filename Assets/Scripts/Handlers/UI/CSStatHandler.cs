using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CSStatHandler : MonoBehaviour {


	public static float MAX = 30;

	public Image[] fillImages;

	private float targetHealth, targetSpeed, targetDamage, targetArmor;

	private CharacterSelectUI characterSelectUI;

	void OnEnable()
	{
		EventManager.OnButtonClick += OnButtonClick;
	}
	void OnDisable()
	{
		EventManager.OnButtonClick -= OnButtonClick;
	}

	void Start()
	{
		characterSelectUI = FindObjectOfType<CharacterSelectUI>();
	}

	void Update ()
	{
		if (GameController.state != State.CHARACTER_SELECT) { return; }
		fillImages[0].fillAmount = Mathf.Lerp(fillImages[0].fillAmount, targetHealth, Time.deltaTime * 10f);
		fillImages[1].fillAmount = Mathf.Lerp(fillImages[1].fillAmount, targetSpeed, Time.deltaTime * 10f);
		fillImages[2].fillAmount = Mathf.Lerp(fillImages[2].fillAmount, targetDamage, Time.deltaTime * 10f);
		fillImages[3].fillAmount = Mathf.Lerp(fillImages[3].fillAmount, targetArmor, Time.deltaTime * 10f);
	}

	public void UpdateValues(CharacterAttributes ca)
	{
		targetHealth = (float)ca.health / MAX;
		targetSpeed = (float)ca.speed / MAX;
		targetDamage = (float)ca.damage / MAX;
		targetArmor = (float)ca.armor / MAX;
	}

	void OnButtonClick(ButtonID id)
	{
		if (id == ButtonID.PERK_ADD_HEALTH)
		{
			CharacterAttributes attributes = PlayerController.Instance.attributes;
			attributes.health++;
			UpdateValues(attributes);
			characterSelectUI.UpdateCharacterSelectUI();
		}
	}
}
