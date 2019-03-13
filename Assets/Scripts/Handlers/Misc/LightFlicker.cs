using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {


	private Light light;

	private float baseRange;

	public float range = 2;

	private float delay;

	public float time = 1;

	private float targetRange;

	private float timer = 0;

	void Start()
	{
		light = GetComponent<Light>();

		baseRange = light.intensity;

		delay = Random.Range(.1f, .2f);
	}

	void Update()
	{
		timer += Time.deltaTime;

		if (timer > delay)
		{
			targetRange = baseRange + Random.Range(-range, range);

			timer = 0;
		}

		light.intensity = Mathf.Lerp(light.intensity, targetRange, Time.deltaTime * 20f);
	}

}
