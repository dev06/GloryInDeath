using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
[System.Serializable]
public struct CameraGameTransform
{
	public Vector3 position;
	public Vector3 rotation;
}

public class CameraController : MonoBehaviour {

	public static CameraController Instance;
	public Light directionalLight;
	public Vector3 cameraOffset;
	public CameraGameTransform cameraGameTransform;

	private Transform _playerTransform;
	private Vector3 _defaultPosition;
	private Vector3 _targetPosition;
	private Quaternion _targetRotation;
	private Canvas _csCanvas;
	private Blur _blur;
	private float _endRotationVelocity;
	private bool _unlockControls;

	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
		EventManager.OnGameEvent += OnGameEvent;
	}
	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
		EventManager.OnGameEvent -= OnGameEvent;
	}
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void Init()
	{
		_playerTransform = FindObjectOfType<MovementHandler>().transform;

		if (_csCanvas == null)
		{
			_csCanvas = transform.GetChild(0).GetComponent<Canvas>();
		}

		_blur = GetComponent<Blur>();

		_csCanvas.enabled = false;

	}

	bool waveEnded;

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.WAVE_END:
			{
				var parameters = new Dictionary<string, object>();
				parameters["WAVE"] = WaveController.Instance.wave;
				//				FacebookManager.Instance.EventSent("Wave Completed", 1, parameters);
				waveEnded = true;
				_endRotationVelocity = (Random.Range(0, 2) == 0 ? -5f : 5f);
				break;
			}
		}
	}

	void OnStateChange(State s)
	{
		if (s == State.GAME)
		{
			//transform.rotation = Quaternion.Euler(cameraGameTransform.rotation);
			_playerTransform = FindObjectOfType<PlayerController>().activeCharacterTransform;

			_defaultPosition = cameraGameTransform.position;
			_targetPosition = _defaultPosition;
			transform.position = _defaultPosition;
			_targetRotation = Quaternion.Euler(cameraGameTransform.rotation);
			transform.rotation = _targetRotation;
			_unlockControls = true;
		}
	}

	public void OnCameraIntroFinished()
	{
		// _targetPosition = cameraGameTransform.position;
		// _targetRotation = Quaternion.Euler(cameraGameTransform.rotation);

		// _defaultPosition = transform.position - new Vector3(0f, 3f, 0);

		// _unlockControls = true;
	}

	void Start ()
	{
		Init();
	}


	void Update ()
	{

		if (GameController.state != State.GAME) { return; }

		if (waveEnded)
		{
			transform.RotateAround(_playerTransform.position, Vector3.up, Time.deltaTime * _endRotationVelocity);

		}
		if (!_unlockControls) { return; }

		Vector3 raw =  _defaultPosition + new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z) + cameraOffset;
		// /raw.z = Mathf.Clamp(raw.z, -15f, raw.z);
		_targetPosition = raw;

		Vector3 relativePos = _playerTransform.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
		_targetRotation = rotation;

		if (!waveEnded)
		{
			transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * 2f);
			transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * 2f);
		}
		else
		{
			_unlockControls = false;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			TriggerShake();
		}
	}

	public void TriggerShake(float i = .5f)
	{
		StopCoroutine("ITriggerShake");
		StartCoroutine("ITriggerShake", i);
	}


	private IEnumerator ITriggerShake(float intensity)
	{
		float jitter = intensity;

		while (jitter > 0)
		{
			transform.position += (Vector3)(Random.insideUnitCircle * jitter);
			jitter -= Time.deltaTime;
			yield return null;
		}

		StopCoroutine("ITriggerJitter");
	}

	public void ToggleBlur(bool b)
	{
		// _blur.enabled = b;
	}



	public void Set_targetPosition(Vector3 position)
	{
		this._targetPosition = position;
	}


}
