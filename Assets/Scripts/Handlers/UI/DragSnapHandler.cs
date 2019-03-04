using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSnapHandler : MonoBehaviour {


	public Transform characterModels;

	private Vector3 currentPosition;

	private Vector3 lastPosition;

	private int selectedIndex;


	private float rot;


	void Start()
	{
		//transform.position = characterModels.position + new Vector3(5f, 0, 0);

		//transform.LookAt(characterModels.GetChild(0).transform.position);
	}

	void Update()
	{
		if (GameController.state != State.CHARACTER_SELECT) return;

		if (Input.GetMouseButton(0))
		{
			Control();

			Camera.main.transform.position += new Vector3(0, 0, rot * 3f);
		}
		else
		{

		}
	}

	void Control()
	{

		currentPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

		if (Input.GetMouseButtonDown(0))
		{
			lastPosition = currentPosition;
		}

		float diff = (currentPosition.x - lastPosition.x) * 10;

		if (diff > 0)
		{
			selectedIndex++;
		}
		else
		{
			selectedIndex--;
		}



		selectedIndex = Mathf.Clamp(selectedIndex, 0, 2);
		//Debug.Log(selectedIndex);

		lastPosition = currentPosition;
	}
}