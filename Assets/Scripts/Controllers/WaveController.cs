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

	private List<Enemy> enemyList = new List<Enemy>();

	public Transform enemyPool, enemyWaveTransform, spawnPoints;

	public int wave = 0; // represents current wave;

	public int waveDifficulty = 5;

	public List<WaveBuilder> waves = new List<WaveBuilder>();

	private int currentEnemyIndex;

	private bool waveEnded;

	void OnEnable()
	{
		EventManager.OnGameEvent += OnGameEvent;
		EventManager.OnStateChange += OnStateChange;
	}
	void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
		EventManager.OnStateChange -= OnStateChange;
	}
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start ()
	{
		//InitializeWave();
	}

	public void StartNextWave()
	{
		StopCoroutine("IStartNextWave");
		StartCoroutine("IStartNextWave");
	}

	private IEnumerator IStartNextWave()
	{
		yield return new WaitForSeconds(.5f);
		wave++;

		InitializeWave();

		StopCoroutine("ISpawnEnemy");

		StartCoroutine("ISpawnEnemy");

		if (EventManager.OnGameEvent != null)
		{
			EventManager.OnGameEvent(EventID.WAVE_START);
		}
	}

	IEnumerator ISpawnEnemy()
	{
		while (currentEnemyIndex < enemyList.Count)
		{

			SpawnNextEnemy();

			yield return new WaitForSeconds(1);

			if (currentEnemyIndex == enemyList.Count - 1)
			{
				StopCoroutine("ISpawnEnemy");
			}
		}

		//StopCoroutine("ISpawnEnemy");
	}


	public void InitializeWave()
	{
		enemyList.Clear();

		currentEnemyIndex = 0;

		WaveBuilder currentWaveBuilder = waves[wave - 1];

		for (int i = 0; i < currentWaveBuilder.enemyInWave.Count; i++)
		{
			for (int j = 0; j < currentWaveBuilder.enemyInWave[i].quantity; j++)
			{
				Enemy e = GetParentTransform(currentWaveBuilder.enemyInWave[i].type).GetChild(j).GetComponent<Enemy>();
				e.Init();
				enemyList.Add(e);
			}
		}

		for (int i = 0; i < enemyList.Count; i++)
		{
			enemyList[i].transform.SetParent(enemyWaveTransform);
		}
	}


	void Update ()
	{
		if (enemyWaveTransform.childCount <= 0)
		{
			if (!waveEnded)
			{

				waveEnded = true;
			}
		}
	}

	public void SpawnNextEnemy()
	{
		if (enemyList[currentEnemyIndex].IsMoving) return;

		enemyList[currentEnemyIndex].transform.position = GetSpawnLocation();
		enemyList[currentEnemyIndex].Move();
		enemyList[currentEnemyIndex].transform.gameObject.SetActive(true);

		currentEnemyIndex++;
		if (currentEnemyIndex > enemyList.Count - 1)
		{
			currentEnemyIndex = enemyList.Count - 1;
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

	private void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.ENEMY_KILLED:
			{
				if (enemyWaveTransform.childCount <= 0)
				{
					if (EventManager.OnGameEvent != null)
					{
						EventManager.OnGameEvent(EventID.WAVE_END);
					}
				}
				break;
			}
		}
	}

	public void ResetEnemyParent(Enemy e)
	{
		Transform parent = GetParentTransform(e.defaultAttributes.type);
		e.transform.SetParent(parent);
		e.transform.position = GetSpawnLocation();
		WaveController.Instance.CheckForWaveEnd();
	}

	public void CheckForWaveEnd()
	{
		if (enemyWaveTransform.childCount <= 0)
		{
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent(EventID.WAVE_END);
			}
		}

	}

	private Vector3 GetSpawnLocation()
	{
		return spawnPoints.GetChild(Random.Range(0, spawnPoints.childCount)).transform.position;
	}

	private void OnStateChange(State s)
	{
		switch (s)
		{
			case State.GAME:
			{
				StartNextWave();
				break;
			}
		}
	}


	IEnumerator IResetEnemyParent()
	{


		// while (enemyWaveTransform.childCount > 0)
		// {
		// 	enemyWaveTransform.GetChild(0).transform.SetParent(GetParentTransform(enemyWaveTransform.GetChild(0).GetComponent<Enemy>().defaultAttributes.type));

		// 	yield return null;
		// }


		yield return new WaitForSeconds(2);
		//InitializeWave();
	}
}
