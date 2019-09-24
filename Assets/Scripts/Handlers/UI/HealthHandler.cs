using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthHandler : MonoBehaviour
{
	public Transform target;
	public Image healthForeground;
	public Image staminaForeground;
	public TextMeshProUGUI healthText;

	[Header("Settings")]
	public Color foregroundColor;

	[HideInInspector]
	public float health, stamina;
	[HideInInspector]
	public string healthString;

	private Vector3 _offset;
	private Transform _camera;
	private CanvasGroup _canvasGroup;
	private Canvas _canvas;

	void OnValidate()
	{
		healthForeground.color = foregroundColor;
	}

	void Start()
	{
		if (target != null) {
			_offset =  transform.localPosition - target.localPosition;
		}

		_canvas = GetComponent<Canvas>();


		_camera = Camera.main.transform;
	}

	void Update()
	{
		if (target != null)
		{
			transform.localPosition = target.localPosition + _offset;
		}

		transform.LookAt(_camera.position);
		healthText.text = healthString;
		healthForeground.fillAmount = health;

		if (staminaForeground != null)
		{
			staminaForeground.fillAmount = stamina;
		}
	}

	public void Toggle(bool b)
	{
		if (_canvasGroup == null)
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}
		_canvasGroup.alpha = b ? 1f : 0f;
	}
}
