using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitDetector : MonoBehaviour {


	void OnTriggerEnter(Collider col)
	{
		if (EventManager.OnWeaponHit != null)
		{
			EventManager.OnWeaponHit(col.gameObject);
		}
	}
}
