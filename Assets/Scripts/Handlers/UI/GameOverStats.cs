using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameOverStats : MonoBehaviour
{

	public TextMeshProUGUI survived, kills, damageDealt, damageTaken;

	public void UpdateData(GameOverStatsSO so)
	{
		survived.text = "Survived - <color=yellow>" + so.timeSurvived;
		kills.text = "Kills - <color=yellow>" + so.kills;
		damageDealt.text = "Damage Dealt - <color=yellow>" + so.damageDealt.ToString("F0");
		damageTaken.text = "Damage Taken - <color=yellow>" + so.damageTaken.ToString("F0");
	}
}
