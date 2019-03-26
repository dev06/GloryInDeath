using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameUI : MonoBehaviour {


	public CanvasGroup waveHUD, waveEndHUD, virtualJoyStickHUD;

	public Image healthForeground, healthBackground;

	public Animation hurtOverlay, fade, goldCollectedAnimation;

	public TextMeshProUGUI goldCollected, healthLeft;


	void OnEnable () {
		EventManager.OnGameEvent += OnGameEvent;
		EventManager.OnButtonClick += OnButtonClick;
		EventManager.OnStateChange += OnStateChange;
	}

	void OnDisable ()
	{
		EventManager.OnGameEvent -= OnGameEvent;
		EventManager.OnButtonClick -= OnButtonClick;
		EventManager.OnStateChange -= OnStateChange;
	}

	void Start()
	{
		Toggle(false);
		healthForeground.fillAmount = PlayerController.Instance.HealthRatio;
		goldCollected.text = 0 + "";
	}

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.WAVE_START:
			{
				StopCoroutine("IShowWaveHUD");
				StartCoroutine("IShowWaveHUD");
				break;
			}

			case EventID.WAVE_END:
			{
				StopCoroutine("IShowWaveEndHUD");
				StartCoroutine("IShowWaveEndHUD");

				break;
			}

			case EventID.PLAYER_HURT:
			{
				healthForeground.fillAmount = PlayerController.Instance.HealthRatio;
				hurtOverlay.Play();
				break;
			}

			case EventID.ENEMY_KILLED:
			{
				WaveController.Instance.GoldCollected += 5;
				goldCollected.text = WaveController.Instance.GoldCollected + "";
				goldCollectedAnimation.Play();
				break;
			}
		}
	}

	void Update()
	{
		healthBackground.fillAmount = Mathf.Lerp(healthBackground.fillAmount, healthForeground.fillAmount, Time.deltaTime);
		healthLeft.text = PlayerController.Instance.attributes.Health + " / " + PlayerController.Instance.defaultAttriubtes.Health;
	}

	void OnStateChange(State s)
	{
		switch (s)
		{
			case State.GAME:
			{
				Toggle(true);
				fade.Play("fade_in");
				break;
			}
		}
	}

	void OnButtonClick(ButtonID id)
	{
	}

	IEnumerator IShowWaveHUD()
	{

		waveHUD.alpha = 1f;

		waveHUD.blocksRaycasts = true;

		waveHUD.transform.GetChild(0).GetComponent<Text>().text = "Wave " + WaveController.Instance.wave;

		waveHUD.transform.GetComponent<Animation>().Play();

		yield return new WaitForSeconds(2);

		while (waveHUD.alpha > .01f)
		{
			waveHUD.alpha -= Time.deltaTime;
			yield return null;
		}

		waveHUD.alpha = 0f;

		waveHUD.blocksRaycasts = false;
	}

	IEnumerator IShowWaveEndHUD()
	{
		CameraController.Instance.ToggleBlur(true);

		virtualJoyStickHUD.alpha = 0f;

		virtualJoyStickHUD.blocksRaycasts = false;

		waveEndHUD.alpha = 1f;

		waveEndHUD.blocksRaycasts = true;

		waveEndHUD.transform.GetComponent<Animation>().Play();

		yield return new WaitForSeconds(2);

		while (waveEndHUD.alpha > .01f)
		{
			waveEndHUD.alpha -= Time.deltaTime;

			yield return null;
		}

		fade.Play("fade_out");

		waveEndHUD.alpha = 0f;

		waveEndHUD.blocksRaycasts = false;

		yield return new WaitForSeconds(1);

		GameController.Instance.Reload();
	}

	private void Toggle(bool b)
	{

		CanvasGroup cg = GetComponent<CanvasGroup>();

		cg.alpha = b ? 1 : 0;

		cg.blocksRaycasts = b;
	}
}
