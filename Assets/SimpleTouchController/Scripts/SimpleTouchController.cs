using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SimpleTouchController : MonoBehaviour {

	// PUBLIC
	public delegate void TouchDelegate(Vector2 value);
	public event TouchDelegate TouchEvent;

	public delegate void TouchStateDelegate(bool touchPresent);
	public event TouchStateDelegate TouchStateEvent;

	// PRIVATE
	[SerializeField]
	private RectTransform joystickArea;
	private bool touchPresent = false;
	private Vector2 movementVector;
	private Vector2 lastTouchVector;

	public Vector2 GetTouchPosition
	{
		get
		{
			movementVector = Vector2.ClampMagnitude(movementVector, 1f);
			if (touchPresent == false)
			{
				movementVector = Vector2.zero;
			}
			return movementVector;
		}
	}
	public Vector2 GetLastTouchVector
	{
		get { return lastTouchVector ;}
	}
	//public void BeginDrag()
	// {

	// 	touchPresent = true;
	// 	if (TouchStateEvent != null) {
	// 		TouchStateEvent(touchPresent);
	// 	}
	// }

	// public void EndDrag()
	// {
	// 	touchPresent = false;
	// 	lastTouchVector = movementVector;
	// 	movementVector = joystickArea.anchoredPosition = Vector2.zero;

	// 	if (TouchStateEvent != null) {
	// 		TouchStateEvent(touchPresent);
	// 	}

	// }
	// public void OnValueChanged(Vector2 value)
	// {
	// 	if (touchPresent)
	// 	{

	// 		// convert the value between 1 0 to -1 +1
	// 		// movementVector.x = ((1 - value.x) - 0.5f) * 2f;
	// 		// movementVector.y = ((1 - value.y) - 0.5f) * 2f;
	// 		// //Debug.Log(movementVector);
	// 		// lastTouchVector = movementVector;
	// 		// Vector3 v = joystickArea.anchoredPosition - Vector2.zero;
	// 		// v = Vector2.ClampMagnitude(v, 70);
	// 		// joystickArea.anchoredPosition = v;


	// 		if (TouchEvent != null)
	// 		{
	// 			TouchEvent(movementVector);
	// 		}
	// 	}

	// }

}
