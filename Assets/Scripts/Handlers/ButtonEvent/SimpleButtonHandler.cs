using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum ButtonID
{
	NONE,
	ATTACK,
	ABILITY,
	CHAR_AURA,
	CHAR_HALFRED,
	CHAR_FREYA,
	SELECT_CHARACTER,
	PERK_ADD_HEALTH,
	PERK_ADD_SPEED,
	PERK_ADD_DAMAGE,
	PERK_ADD_ARMOR,

	UPG_HEALTH,
	UPG_STAMINA,
	UPG_DAMAGE,
	UPG_CRITICAL,

	DEFENSE,
	MENU,
}

[System.Serializable]
public struct Interact
{
	public bool bubble;
	public bool press;
}
public class SimpleButtonHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler {

	public ButtonID buttonID;

	public Interact interact;

	public virtual void OnPointerClick(PointerEventData data)
	{

		if (EventManager.OnButtonClick != null)
		{
			EventManager.OnButtonClick(buttonID, this);
		}

		if (interact.press)
		{
			//GetComponent<Animation>().Play();
		}
	}

	IEnumerator IReset()
	{
		yield return new WaitForEndOfFrame();
		GetComponent<Shadow>().enabled = true;
	}

	public virtual void OnPointerEnter(PointerEventData data)
	{
		if (EventManager.OnButtonEnter != null)
		{
			EventManager.OnButtonEnter(buttonID, this);
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
	public virtual void OnPointerDown(PointerEventData data)
	{

	}

	public virtual void OnPointerUp(PointerEventData data)
	{

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
