using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
[System.Serializable]
public struct WaveBuilder
{
	public List<EnemyWaveBuilder> enemyInWave;
}

public class WaveController : MonoBehaviour {

	public static WaveController Instance;

	public Transform enemyPool, enemyWaveTransform;

	public int wave = 0; // represents current wave;

	public int waveDifficulty = 5;

	public List<WaveBuilder> waves = new List<WaveBuilder>();

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start ()
	{
		InitializeWave();
	}


	public void InitializeWave()
	{

		currentEnemyIndex = 0;

		WaveBuilder currentWaveBuilder = waves[wave];

		for (int i = 0; i < currentWaveBuilder.enemyInWave.Count; i++)
		{
			switch (currentWaveBuilder.enemyInWave[i].type)
			{
				case EnemyType.ORC:
				{
					for (int j = 0; j <  currentWaveBuilder.enemyInWave[i].quantity; j++)
					{
						Enemy e = enemyPool.GetChild(1).GetChild(0).GetComponent<Enemy>();
						e.transform.SetParent(enemyWaveTransform);
						e.Init();
					}
					break;
				}
			}
		}
	}

	private int currentEnemyIndex;
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			SpawnNextEnemy();
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			StopCoroutine("IResetEnemyParent");
			StartCoroutine("IResetEnemyParent");
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			InitializeWave();
		}
	}

	public void SpawnNextEnemy()
	{
		enemyWaveTransform.GetChild(currentEnemyIndex).GetComponent<Enemy>().Move();
		enemyWaveTransform.GetChild(currentEnemyIndex).GetComponent<Enemy>().transform.gameObject.SetActive(true);
		currentEnemyIndex++;
		if (currentEnemyIndex > enemyWaveTransform.childCount - 1)
		{
			currentEnemyIndex = enemyWaveTransform.childCount - 1;
		}
	}

	public Transform GetParentTransform(EnemyType type)
	{
		switch (type)
		{
			case EnemyType.GOBLIN:
			{
				return enemyPool.GetChild(0).transform;
			}

			case EnemyType.ORC:
			{
				return enemyPool.GetChild(1).transform;
			}
		}

		return null;
	}

	IEnumerator IResetEnemyParent()
	{


		while (enemyWaveTransform.childCount > 0)
		{
			enemyWaveTransform.GetChild(0).transform.SetParent(GetParentTransform(enemyWaveTransform.GetChild(0).GetComponent<Enemy>().defaultAttributes.type));

			yield return null;
		}
	}
}
