using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity.Mobile;
public class FacebookManager : MonoBehaviour {

	public static FacebookManager Instance;
	private static bool isInit;
	void Start()
	{

		DontDestroyOnLoad(gameObject);
		if (Instance == null)
		{
			Instance = this;

			Facebook.Unity.FB.Init(InitCallback, OnHideUnity);
		}
		else
		{
			Destroy(gameObject);
		}




	}


	private void InitCallback ()
	{
		if (Facebook.Unity.FB.IsInitialized) {
			// Signal an app activation App Event
			Facebook.Unity.FB.ActivateApp();
			isInit = true;
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}



	public bool EventSent(string name)
	{
		if (!isInit)
		{
			Debug.Log("Event Not Sent");
			return false;
		}

		Facebook.Unity.FB.LogAppEvent(name, 1);

		return true;
	}

	public bool EventSent(string name, int valueToSum,  Dictionary<string, object> parameter)
	{
		if (!isInit)
		{
			Debug.Log("Event Not Sent");
			return false;
		}

		Facebook.Unity.FB.LogAppEvent(name, valueToSum, parameter);

		return true;
	}
}
