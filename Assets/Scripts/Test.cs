using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Test : MonoBehaviour, IDragHandler
{
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			transform.position = Input.mousePosition;
		}
	}
	public void OnDrag(PointerEventData data)
	{

	}
}
