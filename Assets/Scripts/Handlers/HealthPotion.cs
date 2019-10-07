using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
	public ParticleSystem[] fx;
	private MeshRenderer _renderer;
	private Collider _collider;
	private Rigidbody _rigidbody;
	public bool active;
	public void Drop(Vector3 _position)
	{
		foreach (ParticleSystem p in fx)
		{
			p.Play();
		}


		if (_renderer == null)
		{
			_renderer = GetComponent<MeshRenderer>();
		}

		if (_rigidbody == null)
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		if (_collider == null)
		{
			_collider = GetComponent<Collider>();
		}
		active = true;
		transform.position = _position;
		_rigidbody.isKinematic = false;
		_renderer.enabled = _collider.enabled = true;
	}

	public void PickUp()
	{
		foreach (ParticleSystem p in fx)
		{
			p.Stop();
		}

		if (_renderer == null)
		{
			_renderer = GetComponent<MeshRenderer>();
		}

		if (_collider == null)
		{
			_collider = GetComponent<Collider>();
		}

		if (_rigidbody == null)
		{
			_rigidbody = GetComponent<Rigidbody>();
		}
		active = false;
		_rigidbody.isKinematic = true;
		_renderer.enabled = _collider.enabled = false;
	}
}
