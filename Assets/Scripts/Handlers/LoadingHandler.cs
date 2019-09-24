using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadingHandler : MonoBehaviour {

	public TextMeshProUGUI text;
	void Start () {

		StartCoroutine("LoadYourAsyncScene");
	}

	IEnumerator LoadYourAsyncScene()
	{

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
		while (!asyncLoad.isDone)
		{
			float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
			text.text = "Loading..." + (progress * 100f).ToString("F0") + "%";
			yield return null;
		}
	}
}
