using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeStat : MonoBehaviour {


	private float MAX = 30;

	public enum Type
	{
		HEALTH,
		SPEED,
		DAMAGE,
		ARMOR,
	}

	public Type type;

	public Text currentStatText;
	public TextMeshProUGUI upgradeCostText;

	private int upgradeCost = 50;


	public Image fillImage;

	private float targetValue;
	private int targetCostValue;

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

	void Start ()
	{
		targetCostValue = upgradeCost;
		upgradeCostText.text = upgradeCost + "";
	}

	void Update ()
	{
		if (GameController.state != State.CHARACTER_SELECT) { return; }

		fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetValue, Time.deltaTime * 10f);
	}


	void OnButtonClick(ButtonID id)
	{
		if (!GameController.Instance.CanPurchase(upgradeCost)) { return; }
		switch (id)
		{
			case ButtonID.PERK_ADD_HEALTH:
			{
				if (type != UpgradeStat.Type.HEALTH) { break; }
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);
				attributes.Health++;
				targetValue = attributes.Health / MAX;
				GameController.Instance.AddGold(-upgradeCost);
				targetCostValue += 50;
				UpdateValues(attributes);
				if (EventManager.OnUpgradeStat != null)
				{
					EventManager.OnUpgradeStat(type);
				}
				break;
			}
			case ButtonID.PERK_ADD_SPEED:
			{
				if (type != UpgradeStat.Type.SPEED) { break; }
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);
				attributes.Speed++;
				targetValue = attributes.Speed / MAX;
				GameController.Instance.AddGold(-upgradeCost);
				targetCostValue += 50;
				UpdateValues(attributes);
				if (EventManager.OnUpgradeStat != null)
				{
					EventManager.OnUpgradeStat(type);
				}
				break;
			}

			case ButtonID.PERK_ADD_DAMAGE:
			{
				if (type != UpgradeStat.Type.DAMAGE) { break; }
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);
				attributes.Damage++;
				targetValue = attributes.Damage / MAX;
				GameController.Instance.AddGold(-upgradeCost);
				targetCostValue += 50;
				UpdateValues(attributes);
				if (EventManager.OnUpgradeStat != null)
				{
					EventManager.OnUpgradeStat(type);
				}
				break;
			}
			case ButtonID.PERK_ADD_ARMOR:
			{
				if (type != UpgradeStat.Type.ARMOR) { break; }
				CharacterAttributes attributes = GetAttributesByCharacterType(CharacterModel.SELECTED_MODEL.modelType);
				attributes.Armor++;
				targetValue = attributes.Armor / MAX;
				GameController.Instance.AddGold(-upgradeCost);
				targetCostValue += 50;
				UpdateValues(attributes);
				if (EventManager.OnUpgradeStat != null)
				{
					EventManager.OnUpgradeStat(type);
				}
				break;
			}
		}
	}

	public void UpdateValues(CharacterAttributes ca)
	{
		switch (type)
		{
			case UpgradeStat.Type.HEALTH:
			{
				targetValue = ca.Health / MAX;
				currentStatText.text = ca.Health.ToString();
				break;
			}

			case UpgradeStat.Type.SPEED:
			{
				targetValue = ca.Speed / MAX;
				currentStatText.text = ca.Speed.ToString();
				break;

			}
			case UpgradeStat.Type.DAMAGE:
			{
				targetValue = ca.Damage / MAX;
				currentStatText.text = ca.Damage.ToString();
				break;

			}
			case UpgradeStat.Type.ARMOR:
			{
				targetValue = ca.Armor / MAX;
				currentStatText.text = ca.Armor.ToString();
				break;
			}
		}
		StopCoroutine("ILerp");
		StartCoroutine("ILerp");
	}


	private IEnumerator ILerp()
	{
		float t = (float)targetCostValue;
		float u = (float)upgradeCost;
		while (Mathf.Abs(t - u) > .01f)
		{
			u = Mathf.Lerp(u, t, Time.deltaTime * 10f);
			upgradeCostText.text = u.ToString("F0");
			yield return null;
		}
		upgradeCost = (int)Mathf.Round(u);
		upgradeCostText.text = upgradeCost.ToString("F0");
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

}
