using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour {

	private Rigidbody _rigidbody;
	private JoyStickReposition _leftController;
	private PlayerController _playerController;

	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_playerController = GetComponent<PlayerController>();
		_leftController = FindObjectOfType<JoyStickReposition>();
	}
	void FixedUpdate ()
	{
		if (GameController.state != State.GAME) { return; }
		if (_playerController.LockMovement) {
			_playerController.walking = false;
			return;
		}

		Vector3 _movement = _leftController.GetTouchPosition;
		_movement = new Vector3(_movement.x, 0f, _movement.y);
		if (_movement.magnitude != 0)
		{
			if (!_playerController.isPlaying("BaseLayer.Attack"))
			{
				_playerController.activeCharacterTransform.forward = _movement * Time.deltaTime * 4f;
			}
		}
		_playerController.walking = _leftController.GetTouchPosition.magnitude > 0;
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
