using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour {

	private Rigidbody rigidbody;

	public SimpleTouchController leftController;
	private PlayerController playerController;

	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		playerController = GetComponent<PlayerController>();
	}

	void FixedUpdate ()
	{

		if (playerController.LockMovement) { return; }

		if (leftController.GetLastTouchVector.magnitude != 0)
		{
			transform.forward = new Vector3(leftController.GetLastTouchVector.x, 0 , leftController.GetLastTouchVector.y);
		}

		transform.Translate(transform.forward * leftController.GetTouchPosition.magnitude * Time.deltaTime * playerController.Speed, Space.World);


		playerController.SetState(leftController.GetTouchPosition.magnitude > 0 ? CharacterState.RUN : CharacterState.IDLE);

#if UNITY_EDITOR

		if (Input.anyKey)
		{

			float x = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");

			Vector3 movement = new Vector3(x, 0, z);

			if (movement.magnitude != 0)
			{
				transform.forward = movement;
				transform.Translate(movement * Time.deltaTime * playerController.Speed, Space.World);
				playerController.SetState(movement.magnitude > 0 ? CharacterState.RUN : CharacterState.IDLE);
			}

		}
#endif



	}



	private Vector2 GetPointerPosition(Vector2 rawPos)
	{
		return Camera.main.ScreenToViewportPoint(rawPos);
	}
}
