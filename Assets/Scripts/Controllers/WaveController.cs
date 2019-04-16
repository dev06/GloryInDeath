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

	public static int MAX_ENEMY_AT_TIME = 2;

	public static WaveController Instance;

	private List<Enemy> enemyList = new List<Enemy>();

	public Transform enemyPool, enemyWaveTransform, spawnPoints;

	public bool spawnEnemy;

	public int wave = 0; // represents current wave;

	public int waveDifficulty = 5;

	public List<WaveBuilder> waves = new List<WaveBuilder>();

	private int currentEnemyIndex;

	private static int goldCollected;

	private bool waveEnded;

	private int enemySpawnDelay = 2;

	private CharacterSelectUI characterSelectUI;

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
		characterSelectUI = FindObjectOfType<CharacterSelectUI>();
	}

	public void StartNextWave()
	{
		StopCoroutine("IStartNextWave");
		StartCoroutine("IStartNextWave");
	}

	private IEnumerator IStartNextWave()
	{
		yield return new WaitForSeconds(.5f);

		InitializeWave();

		if (spawnEnemy)
		{
			StopCoroutine("ISpawnEnemy");

			StartCoroutine("ISpawnEnemy");
		}


		if (EventManager.OnGameEvent != null)
		{
			EventManager.OnGameEvent(EventID.WAVE_START);
		}
	}

	IEnumerator ISpawnEnemy()
	{
		while (currentEnemyIndex < enemyList.Count)
		{
			int a = GetActiveEnemyCount();

			if (a < MAX_ENEMY_AT_TIME)
			{
				SpawnNextEnemy();
			}

			yield return new WaitForSeconds(enemySpawnDelay);

			if (currentEnemyIndex == enemyList.Count)
			{
				StopCoroutine("ISpawnEnemy");
				Debug.Log("Stopped");
			}
		}
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
		if (enemyList[currentEnemyIndex].IsMoving) { return; }

		enemyList[currentEnemyIndex].transform.position = GetSpawnLocation();

		enemyList[currentEnemyIndex].Move();

		enemyList[currentEnemyIndex].transform.gameObject.SetActive(true);

		currentEnemyIndex++;

		if (currentEnemyIndex > enemyList.Count - 1)
		{
			currentEnemyIndex = enemyList.Count - 1;
		}
	}

	public int GetActiveEnemyCount()
	{
		int alive = 0;

		foreach (Transform t in enemyWaveTransform)
		{
			Enemy e = t.GetComponent<Enemy>();

			if (!e.gameObject.activeSelf) { continue; }

			alive++;
		}

		return alive;
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
			wave++;
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

			case State.CHARACTER_SELECT:
			{

				if (GoldCollected > 0)
				{
					if (characterSelectUI == null)
					{
						characterSelectUI = FindObjectOfType<CharacterSelectUI>();
					}

					characterSelectUI.TriggerGoldCollectedDialog();
				}
				break;
			}
			case State.GAME:
			{
				GoldCollected = 0;
				StartNextWave();
				break;
			}
		}
	}

	public int GoldCollected
	{
		get
		{
			return goldCollected;
		}

		set { goldCollected = value; }
	}

	IEnumerator IResetEnemyParent()
	{
		yield return new WaitForSeconds(2);
	}

	public List<Enemy> EnemyList
	{
		get { return enemyList;}
	}
}
