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

	private Transform playerTransform;

	private Vector3 defaultPosition;

	private Vector3 targetPosition;

	private Canvas csCanvas;

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
		defaultPosition = transform.position;
		targetPosition = transform.position;
	}

	void OnStateChange(State s)
	{

		if (csCanvas == null)
		{
			csCanvas = transform.GetChild(0).GetComponent<Canvas>();
		}

		if (s == State.GAME)
		{
			transform.position = cameraGameTransform.position;
			defaultPosition = transform.position;
			transform.rotation = Quaternion.Euler(cameraGameTransform.rotation);

			csCanvas.enabled = false;
		}
		else if (s == State.CHARACTER_SELECT)
		{
			csCanvas.enabled = true;
		}
	}

	void Start ()
	{
		Init();
	}
	public float defaultFOV = 50f;
	// Update is called once per frame
	void Update ()
	{

		if (GameController.state != State.GAME) { return; }

		//	Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
		transform.position = defaultPosition + playerTransform.position;
	}

	public void SetTargetPosition(Vector3 position)
	{
		this.targetPosition = position;
	}

	void FixedUpdate()
	{
		// RaycastHit[] hits;
		// hits = Physics.RaycastAll(transform.position, transform.forward, 10.0F);

		// for (int i = 0; i < hits.Length; i++)
		// {
		// 	RaycastHit hit = hits[i];
		// 	if (hit.transform.gameObject.tag != "Wall") continue;
		// 	Renderer rend = hit.transform.GetComponent<Renderer>();

		// 	if (rend)
		// 	{
		// 		rend.material.shader = Shader.Find("Transparent/Diffuse");
		// 		Color tempColor = rend.material.color;
		// 		tempColor.a = 0.3F;
		// 		rend.material.color = tempColor;
		// 	}
		// }
	}
}
