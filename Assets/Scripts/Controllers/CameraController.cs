using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraGameTransform
{
	public Vector3 position;
	public Vector3 rotation;
}

public class CameraController : MonoBehaviour {

	public static CameraController Instance;

	public Light directionalLight;

	private Transform playerTransform;

	private Vector3 defaultPosition;

	private Vector3 targetPosition;

	public CameraGameTransform cameraGameTransform;

	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
	}
	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
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
		playerTransform = FindObjectOfType<MovementHandler>().transform;
	}

	void OnStateChange(State s)
	{

		// if (csCanvas == null)
		// {
		// 	csCanvas = transform.GetChild(0).GetComponent<Canvas>();
		// }

		if (s == State.GAME)
		{

			transform.position = cameraGameTransform.position;

			defaultPosition = transform.position;

			transform.rotation = Quaternion.Euler(cameraGameTransform.rotation);

		}
	}

	void Start ()
	{
		Init();
	}


	void Update ()
	{

		if (GameController.state != State.GAME) { return; }

		Vector3 raw =  defaultPosition + playerTransform.position;

		raw.z = Mathf.Clamp(raw.z, -17f, raw.z);

		transform.position = raw;

		transform.LookAt(playerTransform.position);

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


	public void SetTargetPosition(Vector3 position)
	{
		this.targetPosition = position;
	}


}
