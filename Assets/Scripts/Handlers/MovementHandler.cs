using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour {

	private Rigidbody _rigidbody;
	public SimpleTouchController leftController;
	private PlayerController _playerController;

	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_playerController = GetComponent<PlayerController>();
	}
	void FixedUpdate ()
	{
		if (GameController.state != State.GAME) { return; }
		if (_playerController.LockMovement) { return; }
		//Debug.Log(_playerController.activeCharacterTransform);
		Vector3 _movement = leftController.GetTouchPosition;
		_movement = new Vector3(_movement.x, 0f, _movement.y);
		if (_movement.magnitude != 0)
		{
			if (!_playerController.isPlaying("BaseLayer.Attack"))
			{
				_playerController.activeCharacterTransform.forward = _movement * Time.deltaTime * 4f;
			}
		}
		_playerController.walking = leftController.GetTouchPosition.magnitude > 0;
		_playerController.AutoAttackTimer = _playerController.walking ? 0 : _playerController.AutoAttackTimer;

#if UNITY_EDITOR

		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{

			float x = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");

			Vector3 movement = new Vector3(x, 0, z);

			_playerController.activeCharacterTransform.localPosition = new Vector3(_playerController.activeCharacterTransform.localPosition.x, .1f, _playerController.activeCharacterTransform.localPosition.z);

			if (movement.magnitude != 0)
			{
				if (!_playerController.isPlaying("BaseLayer.Attack"))
				{
					_playerController.activeCharacterTransform.forward = Vector3.Lerp(_playerController.activeCharacterTransform.forward, movement, Time.deltaTime * 10f);
					//transform.Translate(movement * Time.deltaTime * 4f, Space.World);
				}
			}

			_playerController.walking = movement.magnitude > 0;
			_playerController.AutoAttackTimer = _playerController.walking ? 0 : _playerController.AutoAttackTimer;
		}
#endif

		_rigidbody.velocity = Vector2.zero;

	}
	private Vector2 GetPointerPosition(Vector2 rawPos)
	{
		return Camera.main.ScreenToViewportPoint(rawPos);
	}
}
