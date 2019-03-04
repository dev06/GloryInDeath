using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonID
{
	NONE,
	ATTACK,
	ABILITY,
	CHAR_AURA,
	CHAR_HALFRED,
	CHAR_FREYA,
}

[System.Serializable]
public struct Interact
{
	public bool bubble;
	public bool press;
}
public class SimpleButtonHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public ButtonID buttonID;

	public Interact interact;

	public virtual void OnPointerClick(PointerEventData data)
	{
		if (EventManager.OnButtonClick != null)
		{
			EventManager.OnButtonClick(buttonID);
		}

		if (interact.press)
		{
			GetComponent<Animation>().Play();
		}
	}

	public virtual void OnPointerEnter(PointerEventData data)
	{
		if (EventManager.OnButtonEnter != null)
		{
			EventManager.OnButtonEnter(buttonID);
		}

		if (interact.bubble)
		{
			Vector3 hoverScale = transform.localScale * 1.1f;
			StopCoroutine("ILerp");
			StartCoroutine("ILerp", hoverScale);
		}
	}

	public virtual void OnPointerExit(PointerEventData data)
	{
		if (interact.bubble)
		{
			StopCoroutine("ILerp");
			StartCoroutine("ILerp", Vector3.one);
		}
	}

	IEnumerator ILerp(Vector3 targetScale)
	{
		while (transform.localScale != targetScale)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 10f);
			yield return null;
		}
	}


}
