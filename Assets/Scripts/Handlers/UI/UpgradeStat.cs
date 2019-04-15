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

	private float targetProgressFill;
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

		UpdateValues(PlayerController.Instance.Attributes);
	}

	void Update ()
	{
		if (GameController.state != State.CHARACTER_SELECT) { return; }

		fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetProgressFill, Time.deltaTime * 10f);
	}


	void OnButtonClick(ButtonID id, SimpleButtonHandler handler)
	{

		switch (id)
		{
			case ButtonID.PERK_ADD_HEALTH:
			{
				if (type != UpgradeStat.Type.HEALTH) { break; }
				CharacterAttributes attributes = CharacterModel.SELECTED_MODEL.attributes;
				if (attributes.index.health >= MAX || !GameController.Instance.CanPurchase(attributes.upgrade.healthCost)) break;
				handler.GetComponent<Animation>().Play();
				GameController.Instance.AddGold((int) - attributes.upgrade.healthCost);
				CharacterModel.SELECTED_MODEL.UpgradeHealth();

				targetProgressFill = attributes.index.health / MAX;
				UpdateValues(attributes);
				EventManager.OnUpgradeStat(type);
				break;
			}
			case ButtonID.PERK_ADD_SPEED:
			{
				if (type != UpgradeStat.Type.SPEED) { break; }
				CharacterAttributes attributes = CharacterModel.SELECTED_MODEL.attributes;
				if (attributes.index.speed >= MAX || !GameController.Instance.CanPurchase(attributes.upgrade.speedCost)) break;

				GameController.Instance.AddGold((int) - attributes.upgrade.speedCost);
				CharacterModel.SELECTED_MODEL.UpgradeSpeed();

				handler.GetComponent<Animation>().Play();
				targetProgressFill = attributes.index.speed / MAX;
				UpdateValues(attributes);
				EventManager.OnUpgradeStat(type);
				break;
			}

			case ButtonID.PERK_ADD_DAMAGE:
			{
				if (type != UpgradeStat.Type.DAMAGE) { break; }
				CharacterAttributes attributes =  CharacterModel.SELECTED_MODEL.attributes;
				if (attributes.index.damage >= MAX || !GameController.Instance.CanPurchase(attributes.upgrade.damageCost)) break;

				GameController.Instance.AddGold((int) - attributes.upgrade.damageCost);
				CharacterModel.SELECTED_MODEL.UpgradeDamage();
				handler.GetComponent<Animation>().Play();
				targetProgressFill = attributes.index.damage / MAX;
				UpdateValues(attributes);
				EventManager.OnUpgradeStat(type);

				break;
			}
			case ButtonID.PERK_ADD_ARMOR:
			{
				if (type != UpgradeStat.Type.ARMOR) { break; }
				CharacterAttributes attributes = CharacterModel.SELECTED_MODEL.attributes;
				if (attributes.index.armor >= MAX || !GameController.Instance.CanPurchase(attributes.upgrade.armorCost)) break;

				GameController.Instance.AddGold((int) - attributes.upgrade.armorCost);
				CharacterModel.SELECTED_MODEL.UpgradeArmor();
				handler.GetComponent<Animation>().Play();
				targetProgressFill = attributes.index.armor / MAX;
				UpdateValues(attributes);
				EventManager.OnUpgradeStat(type);
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
				targetProgressFill = ca.index.health / MAX;
				currentStatText.text = ca.index.health.ToString();
				upgradeCostText.text = ca.index.health >= MAX ? "-" : ca.upgrade.healthCost.ToString("F0");
				break;
			}

			case UpgradeStat.Type.SPEED:
			{
				targetProgressFill = ca.index.speed / MAX;
				currentStatText.text = ca.index.speed.ToString();
				upgradeCostText.text = ca.index.speed >= MAX ? "-" : ca.upgrade.speedCost.ToString("F0");
				break;

			}
			case UpgradeStat.Type.DAMAGE:
			{
				targetProgressFill = ca.index.damage / MAX;
				currentStatText.text = ca.index.damage.ToString();
				upgradeCostText.text = ca.index.damage >= MAX ? "-" : ca.upgrade.damageCost.ToString("F0");
				break;

			}
			case UpgradeStat.Type.ARMOR:
			{
				targetProgressFill = ca.index.armor / MAX;
				currentStatText.text = ca.index.armor.ToString();
				upgradeCostText.text = ca.index.armor >= MAX ? "-" : ca.upgrade.armorCost.ToString("F0");
				break;
			}
		}
		//StopCoroutine("ILerp");
		//StartCoroutine("ILerp");
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

	// public CharacterAttributes GetAttributesByCharacterType(CharacterType type)
	// {
	// 	switch (type)
	// 	{
	// 		case CharacterType.AURA_BLACKSWORD:
	// 		{
	// 			return CharacterModifiedLibrary.CHAR_AURA_BLACKSWORD;
	// 		}
	// 		case CharacterType.HALLFRED_THORALDSON:
	// 		{
	// 			return CharacterModifiedLibrary.CHAR_HALLFRED_THORALDSON;
	// 		}
	// 		case CharacterType.FREYA_SKAAR:
	// 		{
	// 			return CharacterModifiedLibrary.CHAR_FREYA_SKAAR;
	// 		}
	// 	}

	// 	return null;
	// }

	public void OnCharacterModelHover(CharacterModel m)
	{
		UpdateValues(PlayerController.Instance.Attributes);
	}
}
