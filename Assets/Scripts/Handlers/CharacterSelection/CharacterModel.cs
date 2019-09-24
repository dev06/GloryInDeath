using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
	public static CharacterModel SELECTED_MODEL;

	public DefaultCharacterAttribute defaultCharacterAttributes;
	public CharacterAttributes attributes;

	private CharacterType modelType;

	void OnEnable()
	{
		EventManager.OnCharacterModelHover += OnCharacterModelHover;
	}
	void OnDisable()
	{
		EventManager.OnCharacterModelHover -= OnCharacterModelHover;
	}

	void Start()
	{
		modelType = defaultCharacterAttributes.type;
		if (PlayerPrefs.HasKey(modelType + ""))
		{
			attributes.SetAttributes(CharacterAttributes.Load(modelType));
		}
		else
		{
			attributes.SetAttributes(defaultCharacterAttributes);
		}
	}

	public void Hover()
	{
		SELECTED_MODEL = this;

		if (EventManager.OnCharacterModelHover != null)
		{
			EventManager.OnCharacterModelHover(SELECTED_MODEL);
		}
	}

	void Update()
	{
		if (this == SELECTED_MODEL)
		{
			transform.Rotate(Vector3.up, -Time.deltaTime * 30f);
		}
	}

	public void OnCharacterModelHover(CharacterModel m)
	{
		if (this == m)
		{
			StopCoroutine("IShrink");
			StopCoroutine("IExpand");
			StartCoroutine("IExpand");
		}
		else
		{
			StopCoroutine("IExpand");
			StopCoroutine("IShrink");
			StartCoroutine("IShrink");
		}
	}

	IEnumerator IExpand()
	{

		Vector3 expandScale = new Vector3(1.2f, 1.2f, 1.2f);
		ToggleChildren(true);

		while (transform.localScale != expandScale)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, expandScale, Time.deltaTime * 10f);
			yield return null;
		}

	}

	IEnumerator IShrink()
	{
		Vector3 target = Vector3.one * .2f;
		Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 90, 0));
		yield return new WaitForSecondsRealtime(.25f);
		ToggleChildren(false);
		while (transform.localScale != target || transform.rotation != targetRotation)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * 10f);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
			yield return null;
		}

	}

	public void ToggleChildren(bool b)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(b);
		}
	}
	public void UpgradeHealth()
	{
		attributes.index.health++;
		attributes.Health = defaultCharacterAttributes.health + (attributes.index.health * 10);
		attributes.upgrade.healthCost += 50;
		attributes.Save();
	}

	public void UpgradeSpeed()
	{
		attributes.index.speed++;
		attributes.Speed = defaultCharacterAttributes.speed + (attributes.index.speed);
		attributes.upgrade.speedCost += 50;
		attributes.Save();
	}

	public void UpgradeDamage()
	{
		attributes.index.damage++;
		attributes.Damage++;
		attributes.upgrade.damageCost += 50;
		attributes.Save();
	}

	public void UpgradeArmor()
	{
		attributes.index.armor++;
		attributes.Armor = defaultCharacterAttributes.armor + (attributes.index.armor);
		attributes.upgrade.armorCost += 50;
		attributes.Save();
	}

	public CharacterType ModelType
	{
		get { return modelType;}
	}
}
