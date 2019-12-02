using System.Collections;
using System.Collections.Generic;
using RabaGames;
using TMPro;
using UnityEngine;
public enum State
{
	CHARACTER_SELECT,
	GAME,
}
public class GameController : MonoBehaviour
{

	public static GameController Instance;
	public static State state = State.CHARACTER_SELECT;
	public static bool PAUSE;
	public int waveStarts = 0;
	public GameOverUI gameOverUI;
	public GameOverStatsSO gameOverStatsSO;
	public bool DeleteSave;

	private int currentGold = 999999999;

	[Header ("Debug")]
	public TextMeshProUGUI fpsText;
	private float _deltatime;
	private float _updateFPSTimer;

	private float _mins;
	private float _secs;
	private bool _startTimer;

	void OnEnable ()
	{
		EventManager.OnGameEvent += OnGameEvent;
		EventManager.OnButtonClick += OnButtonClick;
	}

	void OnDisable ()
	{
		EventManager.OnGameEvent -= OnGameEvent;
		EventManager.OnButtonClick -= OnButtonClick;
	}

	void Awake ()
	{
		Application.targetFrameRate = 60;

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy (gameObject);
		}
		Load ();
		SetState (State.CHARACTER_SELECT);

		//Debug.Log("Hit");

	}

	void OnValidate ()
	{
		if (DeleteSave)
		{
			PlayerPrefs.DeleteAll ();
		}
	}

	void Start ()
	{
		if (waveStarts > 0 && waveStarts % 5 == 0)
		{
			ShowRateDialog ();
		}

		gameOverStatsSO.timeSurvived = "0:00";
		gameOverStatsSO.kills = 0;
		gameOverStatsSO.damageDealt = 0;
		gameOverStatsSO.damageTaken = 0;
	}

	void Update ()
	{
		_deltatime += (Time.unscaledDeltaTime - _deltatime) * .1f;
		_updateFPSTimer += Time.deltaTime;
		float msec = _deltatime * 1000f;
		float fps = 1f / _deltatime;
		if (_updateFPSTimer > 1f)
		{
			fpsText.text = (int) fps + " fps";
			_updateFPSTimer = 0f;
		}

		if (_startTimer)
		{
			_secs += Time.deltaTime;
			if (_secs > 59)
			{
				_secs = 0;
				_mins++;
			}
		}
	}

	void OnGameEvent (EventID id)
	{
		if (id == EventID.WAVE_START)
		{
			_startTimer = true;
		}

		if (id == EventID.DEATH)
		{
			_startTimer = false;
			gameOverStatsSO.timeSurvived = TimeSurvived;
			gameOverUI.Display (gameOverStatsSO);
		}
	}

	void OnButtonClick (ButtonID id, SimpleButtonHandler handler)
	{
		if (id == ButtonID.MENU)
		{
			Reload ();
		}
	}

	public void ShowRateDialog ()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
#if UNITY_IOS
			RateInsideAppiOS.DisplayReviewDialog ();
#endif
		}
	}

	public void SetState (State s)
	{
		state = s;

		if (EventManager.OnStateChange != null)
		{
			EventManager.OnStateChange (state);
		}
	}

	public static Vector3 ClampMagnitude (Vector3 v, float min, float max)
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

	private void Save ()
	{
		PlayerPrefs.SetInt ("WAVE_COMPLETED", WaveController.Instance.wave);
		PlayerPrefs.SetInt ("WAVESTARTS", waveStarts);
	}

	private void Load ()
	{
		PAUSE = false;
		WaveController.Instance.wave = PlayerPrefs.HasKey ("WAVE_COMPLETED") ? PlayerPrefs.GetInt ("WAVE_COMPLETED") : 1;
		CurrentGold = PlayerPrefs.HasKey ("GOLD") ? PlayerPrefs.GetInt ("GOLD") : 100;
		waveStarts = PlayerPrefs.HasKey ("WAVESTARTS") ? PlayerPrefs.GetInt ("WAVESTARTS") : 0;

		// WaveController.Instance.wave = 21;
	}

	public void Reload ()
	{
		Save ();
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public void AddGold (int gold)
	{
		CurrentGold += gold;
		PlayerPrefs.SetInt ("GOLD", CurrentGold);
	}

	public bool CanPurchase (float cost)
	{
		return CurrentGold >= (int) cost;
	}

	private string convert (float num)
	{
		return num < 10 ? "0" + num.ToString ("F0") : "" + num.ToString ("F0");
	}

	public string TimeSurvived
	{
		get { return convert (_mins) + ":" + convert (_secs); }
	}
}