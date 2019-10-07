using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{

	public GameOverStats gameOverStats;

	public void Display(GameOverStatsSO gameOverStatsSO)
	{
		gameOverStats.UpdateData(gameOverStatsSO);
		GetComponent<CanvasGroup>().alpha = 1f;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
