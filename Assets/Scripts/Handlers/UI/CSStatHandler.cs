using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct InfoText
{
	public Text name;
	public Text health;
	public Text speed;
	public Text damage;
	public Text armor;
}


public class CSStatHandler : MonoBehaviour {


	public static float MAX = 30;

	public Image[] fillImages;

	private float targetHealth, targetSpeed, targetDamage, targetArmor;

	private CharacterSelectUI characterSelectUI;

	public InfoText characterInfoText;

	void OnEnable()
	{
		EventManager.OnButtonClick += OnButtonClick;
		EventManager.OnCharacterModelHover += OnCharacterModelHover;
	}
	void OnDisable()
	{
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnCharacterModelHover -= OnCharacterModelHover;
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


		characterInfoText.health.text = ca.health.ToString();
		characterInfoText.speed.text = ca.speed.ToString();
		characterInfoText.damage.text = ca.damage.ToString();
		characterInfoText.armor.text = ca.armor.ToString();
	}

	public void OnCharacterModelHover(CharacterModel m)
	{
		switch (CharacterModel.SELECTED_MODEL.modelType)
		{
			case CharacterType.AURA_BLACKSWORD:
			{
				UpdateValues(CharacterModifiedLibrary.CHAR_AURA_BLACKSWORD);
				break;
			}
			case CharacterType.HALLFRED_THORALDSON:
			{
				UpdateValues(CharacterModifiedLibrary.CHAR_HALLFRED_THORALDSON);
				break;
			}
			case CharacterType.FREYA_SKAAR:
			{

				UpdateValues(CharacterModifiedLibrary.CHAR_FREYA_SKAAR);

				break;
			}

		}

	}

	void OnButtonClick(ButtonID id)
	{

		switch (id)
		{
			case ButtonID.PERK_ADD_HEALTH:
			{
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);

				attributes.Health++;

				UpdateValues(attributes);

				break;
			}

			case ButtonID.PERK_ADD_SPEED:
			{
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);

				attributes.Speed++;

				UpdateValues(attributes);

				break;
			}
			case ButtonID.PERK_ADD_DAMAGE:
			{
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);

				attributes.Damage++;

				UpdateValues(attributes);

				break;
			}
			case ButtonID.PERK_ADD_ARMOR:
			{
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);

				attributes.Armor++;

				UpdateValues(attributes);

				break;
			}
		}
	}

	public CharacterAttributes GetAttributesByCharacterType(CharacterType type)
	{
		switch (type)
		{
			case CharacterType.AURA_BLACKSWORD:
			{
				return CharacterModifiedLibrary.CHAR_AURA_BLACKSWORD;
			}
			case CharacterType.HALLFRED_THORALDSON:
			{
				return CharacterModifiedLibrary.CHAR_HALLFRED_THORALDSON;
			}
			case CharacterType.FREYA_SKAAR:
			{
				return CharacterModifiedLibrary.CHAR_FREYA_SKAAR;
			}
		}

		return null;
	}
}
