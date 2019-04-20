using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapticPlugin;
public class Haptic : MonoBehaviour {

	public static Haptic Instance;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public static bool ShowLog = false;

	public static bool Enabled = true;

	public static void Vibrate(HapticIntensity intensity)
	{

		if (ShowLog) Debug.Log("Vibrate " + intensity);

		if (SystemInfo.supportsVibration == false) return;

		if (!Enabled) return;


#if UNITY_IPHONE
		try
		{
			switch (intensity)
			{
				case HapticIntensity.Light:
				{
					TapticManager.Impact(ImpactFeedback.Light);
					break;
				}
				case HapticIntensity.Medium:
				{
					TapticManager.Impact(ImpactFeedback.Midium);
					break;
				}
				case HapticIntensity.Heavy:
				{
					TapticManager.Impact(ImpactFeedback.Heavy);
					break;
				}
			}
		}
		catch (System.Exception e)
		{

		}

#else
		try
		{
			switch (intensity)
			{
				case HapticIntensity.Light:
				{
					Vibration.Vibrate(10);
					break;
				}
				case HapticIntensity.Medium:
				{
					Vibration.Vibrate(15);
					break;
				}
				case HapticIntensity.Heavy:
				{
					Vibration.Vibrate(20);
					break;
				}
			}
		}
		catch (System.Exception e)
		{

		}

#endif

	}

	public void VibrateTwice(float delay, HapticIntensity intensity)
	{

		if (!Enabled) return;

		StopCoroutine("IVibrateTwice");
		StartCoroutine("IVibrateTwice", new object[2] {delay, intensity});
	}

	IEnumerator IVibrateTwice(object[] obj)
	{
		float delay = (float)obj[0];
		HapticIntensity intensity = (HapticIntensity)obj[1];

		Haptic.Vibrate(intensity);
		yield return new WaitForSeconds(delay);
		Haptic.Vibrate(intensity);
	}

	public static void VibrateHandheld()
	{
		if (!Enabled) return;
		try
		{
			if (SystemInfo.supportsVibration)
			{
#if !UNITY_WEBGL

				Handheld.Vibrate();
#endif
			}
		}
		catch (System.Exception e)
		{

		}
	}
}



public enum HapticIntensity
{
	Light,
	Medium,
	Heavy,
}
