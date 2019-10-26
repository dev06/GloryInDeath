using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeCard : MonoBehaviour
{
	public Image icon;
	public TextMeshProUGUI title;
	private SimpleButtonHandler button;
	private IngameUpgrade ingameUpgrade;
	public int counter;
	public float delay, multiplier;
	void Start()
	{
		button = GetComponent<SimpleButtonHandler>();
		ingameUpgrade = GetComponentInParent<IngameUpgrade>();
	}

	public void Set(CharacterUpgrade upgradeInfo)
	{
		icon.enabled = title.enabled = true;
		icon.sprite = upgradeInfo.icon;
		title.text = upgradeInfo.title;
		button.buttonID = upgradeInfo.buttonID;
	}

	public void Shuffle()
	{
		Clear();
		StartCoroutine("IShuffle");
	}

	IEnumerator IShuffle()
	{
		int index = Random.Range(0, ingameUpgrade.activeUpgrades.Count);
		int count = 0;
		while (count < counter * multiplier)
		{
			CharacterUpgrade upg = ingameUpgrade.activeUpgrades[index];
			Set(upg);
			index++;
			if (index > ingameUpgrade.activeUpgrades.Count - 1)
			{
				index = 0;
			}
			count++;
			yield return new WaitForSecondsRealtime(delay);
		}
	}


	public void Clear()
	{
		icon.enabled = false;
		title.enabled = false;
	}
}
