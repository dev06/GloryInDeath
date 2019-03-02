using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public static CameraController Instance;

	private Transform playerTransform;

	private Vector3 defaultPosition;
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
	}

	void Start ()
	{
		Init();
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position = defaultPosition + playerTransform.position;
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
