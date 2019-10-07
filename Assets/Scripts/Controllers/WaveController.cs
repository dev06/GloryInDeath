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

	public static int MAX_ENEMY_AT_TIME = 4;
	public static WaveController Instance;
	public List<WaveBuilder> waves = new List<WaveBuilder>();
	public bool spawnEnemy;
	public int wave = 0; // represents current wave;
	public int waveDifficulty = 5;
	public Transform enemyPool, enemyWaveTransform, spawnPoints;

	private static int goldCollected;
	private CharacterSelectUI characterSelectUI;
	private List<Enemy> enemyList = new List<Enemy>();
	private int currentEnemyIndex;
	private int enemySpawnDelay = 0;
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
		characterSelectUI = FindObjectOfType<CharacterSelectUI>();
	}

	public void StartNextWave()
	{
		GameController.Instance.waveStarts++;
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
		while (true)
		{
			int a = GetActiveEnemyCount();

			if (a < MAX_ENEMY_AT_TIME)
			{
				SpawnNextEnemy();
			}

			yield return new WaitForSeconds(enemySpawnDelay);
		}
	}


	public void InitializeWave()
	{
		enemyList.Clear();

		currentEnemyIndex = 0;

		WaveBuilder currentWaveBuilder;

		for (int i = 0; i < 2; i++)
		{
			Enemy e = GetParentTransform(EnemyType.GOBLIN).GetChild(i).GetComponent<Enemy>();
			e.Init();
			e.transform.SetParent(enemyWaveTransform);
		}

		for (int i = 0; i < 2; i++)
		{
			Enemy e = GetParentTransform(EnemyType.ORC).GetChild(i).GetComponent<Enemy>();
			e.Init();
			e.transform.SetParent(enemyWaveTransform);
		}
	}


	void Update ()
	{
		if (GameController.state != State.GAME) { return; }

		if (Input.GetKeyDown(KeyCode.R)) {
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent(EventID.WAVE_END);
			}
			waveEnded = true;
		}
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
		Enemy e = GetNextEnemy();
		if (e == null) { return; }
		if (e.IsMoving) { return; }

		e.transform.position = GetSpawnLocation();

		e.Move();

		e.transform.gameObject.SetActive(true);

		// currentEnemyIndex++;

		// if (currentEnemyIndex > enemyWaveTransform.childCount - 1)
		// {
		// 	currentEnemyIndex = enemyWaveTransform.childCount - 1;
		// }

	}

	public Enemy GetNextEnemy()
	{
		for (int i = 0; i < enemyWaveTransform.childCount; i++)
		{
			if (enemyWaveTransform.GetChild(i).gameObject.activeSelf)
			{
				continue;
			}

			return enemyWaveTransform.GetChild(i).GetComponent<Enemy>();
		}

		return null;
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
					waveEnded = true;
				}

				break;
			}
		}
	}

	public void ResetEnemyParent(Enemy e)
	{
		Transform parent = GetParentTransform(e.Attributes.type);
		e.transform.SetParent(parent);
		e.transform.position = GetSpawnLocation();
		e.transform.gameObject.SetActive(false);

		EnemyType eType = Random.value > .8f ? EnemyType.GOBLIN : EnemyType.ORC;
		Enemy ee = GetParentTransform(eType).GetChild(0).GetComponent<Enemy>();
		ee.Init();
		ee.transform.SetParent(enemyWaveTransform);

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

	public Transform EnemyWaveTransform
	{
		get {return enemyWaveTransform; }
	}


	public List<Enemy> EnemyList
	{
		get { return enemyList;}
	}
}
