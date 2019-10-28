using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum ButtonID
{
	NONE = 0,
	ATTACK = 1,
	ABILITY = 2,
	CHAR_AURA = 3,
	CHAR_HALFRED = 4,
	CHAR_FREYA = 5,
	SELECT_CHARACTER = 6,
	PERK_ADD_HEALTH = 7,
	PERK_ADD_SPEED = 8,
	PERK_ADD_DAMAGE = 9,
	PERK_ADD_ARMOR = 10,

	UPG_HEALTH = 11,
	UPG_STAMINA = 12,
	UPG_DAMAGE = 13,
	UPG_CRITICAL = 14,
	UPG_STAMINA_REGEN = 22,

	DEFENSE = 15,
	MENU = 16,

	B_UPG_CRITHIT = 17,
	B_UPG_DAMAGE = 18,
	B_UPG_ACTIVATE = 19,
	B_UPG_HEALTH = 23, //*

	B_STGS_CLOSE = 20,
	B_STGS_OPEN = 21,

}

[System.Serializable]
public struct Interact
{
	public bool bubble;
	public bool press;
}
public class SimpleButtonHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{

	public ButtonID buttonID;

	public Interact interact;

	public virtual void OnPointerClick (PointerEventData data)
	{

		if (EventManager.OnButtonClick != null)
		{
			EventManager.OnButtonClick (buttonID, this);
		}

		if (interact.press)
		{
			//GetComponent<Animation>().Play();
		}
	}

	IEnumerator IReset ()
	{
		yield return new WaitForEndOfFrame ();
		GetComponent<Shadow> ().enabled = true;
	}

	public virtual void OnPointerEnter (PointerEventData data)
	{
		if (EventManager.OnButtonEnter != null)
		{
			EventManager.OnButtonEnter (buttonID, this);
		}

		if (interact.bubble)
		{
			Vector3 hoverScale = transform.localScale * 1.1f;
			StopCoroutine ("ILerp");
			StartCoroutine ("ILerp", hoverScale);
		}
	}

	public virtual void OnPointerExit (PointerEventData data)
	{
		if (interact.bubble)
		{
			StopCoroutine ("ILerp");
			StartCoroutine ("ILerp", Vector3.one);
		}

	}
	public virtual void OnPointerDown (PointerEventData data)
	{
		if (EventManager.OnButtonDown != null)
		{
			EventManager.OnButtonDown (buttonID, this);
		}
	}

	public virtual void OnPointerUp (PointerEventData data)
	{
		if (EventManager.OnButtonUp != null)
		{
			EventManager.OnButtonUp (buttonID, this);
		}
	}

	IEnumerator ILerp (Vector3 targetScale)
	{
		while (transform.localScale != targetScale)
		{
			transform.localScale = Vector3.Lerp (transform.localScale, targetScale, Time.deltaTime * 10f);
			yield return null;
		}
	}

}