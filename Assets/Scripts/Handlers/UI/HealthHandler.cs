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
	public Image levelForeground;
	public TextMeshProUGUI healthText, levelText;

	[Header("Settings")]
	public Color foregroundColor;
	public bool lerpColor;
	public Color lerpToColor;

	[HideInInspector]
	public float health, stamina, levelProgress;
	[HideInInspector]
	public string healthString, levelTextString;

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
		if (lerpColor) {
			healthForeground.color = Color.Lerp(foregroundColor, lerpToColor, 1f - health);
		}
		if (staminaForeground != null)
		{
			staminaForeground.fillAmount = stamina;
		}

		if (levelText != null)
		{
			levelText.text = levelTextString;
		}

		if (levelForeground != null)
		{
			levelForeground.fillAmount = levelProgress;

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
