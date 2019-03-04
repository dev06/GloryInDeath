using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
	CHARACTER_SELECT,
	GAME,
}
public class GameController : MonoBehaviour {


	public static GameController Instance;
	public static State state = State.CHARACTER_SELECT;

	public bool DeleteSave;

	void Awake()
	{
		Application.targetFrameRate = 60;

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		SetState(State.CHARACTER_SELECT);
	}

	void OnValidate()
	{
		if (DeleteSave)
		{
			PlayerPrefs.DeleteAll();
		}
	}

	void Start()
	{
		Load();
	}

	public void SetState(State s)
	{
		state = s;

		if (EventManager.OnStateChange != null)
		{
			EventManager.OnStateChange(state);
		}
	}

	public static Vector3 ClampMagnitude(Vector3 v, float min, float max)
	{
		float m = v.sqrMagnitude;

		if (m > max * max)
		{
			return v.normalized * max;
		}

		if (m < min * min)
		{
			return v.normalized * min;
		}

		return v;
	}

	private void Save()
	{
		PlayerPrefs.SetInt("WAVE_COMPLETED", WaveController.Instance.wave);
	}

	private void Load()
	{
		WaveController.Instance.wave = PlayerPrefs.HasKey("WAVE_COMPLETED") ? PlayerPrefs.GetInt("WAVE_COMPLETED") : 0;
	}

	public void Reload()
	{
		Save();
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
