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

	public float distanceOffset = 0;

	private Transform playerTransform;

	private Vector3 defaultPosition;

	private Vector3 targetPosition;

	private Quaternion targetRotation;

	public CameraGameTransform cameraGameTransform;

	private bool unlockControls;

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
			GetComponent<Animation>().Play();

			transform.rotation = Quaternion.Euler(cameraGameTransform.rotation);
		}
	}

	public void OnCameraIntroFinished()
	{
		targetPosition = cameraGameTransform.position;
		targetRotation = Quaternion.Euler(cameraGameTransform.rotation);

		defaultPosition = transform.position;

		unlockControls = true;
	}

	void Start ()
	{
		Init();
	}


	void Update ()
	{

		if (GameController.state != State.GAME) { return; }

		if (!unlockControls) return;

		Vector3 raw =  defaultPosition + new Vector3(playerTransform.position.x, 0, playerTransform.position.z) + new Vector3(0, 0, distanceOffset);
		raw.z = Mathf.Clamp(raw.z, -17f, raw.z);
		targetPosition = raw;

		Vector3 relativePos = playerTransform.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
		targetRotation = rotation;

		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

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
