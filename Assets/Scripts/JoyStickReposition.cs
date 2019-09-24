using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]
public class JoyStickReposition : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public TextMeshProUGUI touchDisplay;
	public RectTransform joystickArea;
	public float radius = 1f;
	public float joystickRadius = 1f;

	private RectTransform _rectTransform;
	private CanvasGroup _canvasGroup;
	private Vector3 _pointerDown;
	private Vector3 _pointerUp, _pointerCurrent;
	private Vector2 _movementVector, _delta;
	private float _defaultSize = 100f;
	private float _calculateSize;
	private bool _touchPresent;
	private int _touchCount;

	void Start()
	{
		_canvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
		_rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
		_calculateSize = .5f * radius * _defaultSize;
		_canvasGroup.alpha = 0f;
	}

	void OnValidate()
	{
		if (_rectTransform == null)
		{
			_rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
		}

		joystickArea.sizeDelta = new Vector3(joystickRadius * _defaultSize, joystickRadius * _defaultSize);
		_rectTransform.sizeDelta = new Vector2(radius * _defaultSize, radius * _defaultSize);
	}
	public void OnPointerUp(PointerEventData data)
	{
		joystickArea.anchoredPosition = _delta = Vector2.zero;
		_canvasGroup.alpha = 0f;
	}
	public void OnPointerDown(PointerEventData data)
	{
		transform.GetChild(0).position = data.position;
		_pointerDown = data.position;
		_canvasGroup.alpha = 1f;
	}
	public void OnDrag(PointerEventData data)
	{
		_pointerCurrent = data.position;
		_delta = _pointerCurrent - _pointerDown;
		_delta = Vector2.ClampMagnitude(_delta, _calculateSize);
		joystickArea.anchoredPosition = _delta;
	}

	public Vector2 GetTouchPosition
	{
		get
		{
			float scaledValueX = Mathf.Clamp(_delta.x / _calculateSize, -_calculateSize, _calculateSize);
			float scaledValueY = Mathf.Clamp(_delta.y / _calculateSize, -_calculateSize, _calculateSize);
			_movementVector.x = scaledValueX;
			_movementVector.y = scaledValueY;
			return _movementVector;
		}
	}
}

