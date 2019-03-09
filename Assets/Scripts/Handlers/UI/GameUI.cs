using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour {


	public CanvasGroup waveHUD, waveEndHUD;

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
		}
	}

	void OnStateChange(State s)
	{
		switch (s)
		{
			case State.GAME:
			{
				Toggle(true);
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
		waveEndHUD.alpha = 1f;

		waveEndHUD.blocksRaycasts = true;

		waveEndHUD.transform.GetComponent<Animation>().Play();

		yield return new WaitForSeconds(2);

		while (waveEndHUD.alpha > .01f)
		{
			waveEndHUD.alpha -= Time.deltaTime;

			yield return null;
		}

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
