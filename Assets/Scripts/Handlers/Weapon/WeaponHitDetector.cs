using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitDetector : MonoBehaviour {


	private TrailRenderer _trail;

	void OnEnable() {
		EventManager.OnRegularAttackStart += OnRegularAttackStart;
		EventManager.OnRegularAttackEnd += OnRegularAttackEnd;
	}
	void OnDisable() {
		EventManager.OnRegularAttackStart -= OnRegularAttackStart;
		EventManager.OnRegularAttackEnd -= OnRegularAttackEnd;
	}
	void OnRegularAttackStart(string _id)
	{
		if (_trail == null) { return; }
		_trail.enabled = true;
	}
	void OnRegularAttackEnd(string _id)
	{
		if (_trail == null) { return; }
		_trail.enabled = false;
	}

	void Start()
	{
		_trail = GetComponentInChildren<TrailRenderer>();
		_trail.enabled = false;
	}


	void OnTriggerEnter(Collider col)
	{
		if (EventManager.OnWeaponHit != null)
		{
			EventManager.OnWeaponHit(col.gameObject);
		}
	}
}
