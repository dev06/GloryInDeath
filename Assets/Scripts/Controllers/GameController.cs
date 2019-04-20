using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RabaGames;
public enum State
{
	CHARACTER_SELECT,
	GAME,
}
public class GameController : MonoBehaviour {


	public static GameController Instance;
	public static State state = State.CHARACTER_SELECT;

	public static bool PAUSE;
	public bool DeleteSave;


	private int currentGold = 999999999;

	public int waveStarts = 0;


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
		Load();
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
		if (waveStarts > 0 && waveStarts % 5 == 0)
		{
			ShowRateDialog();
		}
	}


	public void ShowRateDialog()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			RateInsideAppiOS.DisplayReviewDialog();
		}
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


	public int CurrentGold
	{
		get { return currentGold; }
		set { this.currentGold = value; }
	}

	private void Save()
	{
		PlayerPrefs.SetInt("WAVE_COMPLETED", WaveController.Instance.wave);
		PlayerPrefs.SetInt("WAVESTARTS", waveStarts);
	}

	private void Load()
	{
		WaveController.Instance.wave = PlayerPrefs.HasKey("WAVE_COMPLETED") ? PlayerPrefs.GetInt("WAVE_COMPLETED") : 1;
		CurrentGold = PlayerPrefs.HasKey("GOLD") ? PlayerPrefs.GetInt("GOLD") : 100;
		waveStarts = PlayerPrefs.HasKey("WAVESTARTS") ?  PlayerPrefs.GetInt("WAVESTARTS") : 0;

		// WaveController.Instance.wave = 21;
	}

	public void Reload()
	{
		Save();
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}


	public void AddGold(int gold)
	{
		CurrentGold += gold;
		PlayerPrefs.SetInt("GOLD", CurrentGold);
	}

	public bool CanPurchase(float cost)
	{
		return CurrentGold >= (int)cost;
	}
}
