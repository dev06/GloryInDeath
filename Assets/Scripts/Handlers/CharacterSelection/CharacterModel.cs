using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
	public static CharacterModel SELECTED_MODEL;

	public CharacterType modelType;

	void OnEnable()
	{
		EventManager.OnCharacterModelHover += OnCharacterModelHover;
	}
	void OnDisable()
	{
		EventManager.OnCharacterModelHover -= OnCharacterModelHover;
	}

	public void Hover()
	{
		SELECTED_MODEL = this;

		PlayerController.Instance.SetCharacter(modelType);

		if (EventManager.OnCharacterModelHover != null)
		{
			EventManager.OnCharacterModelHover(SELECTED_MODEL);
		}
	}

	void Update()
	{
		if (this == SELECTED_MODEL)
		{
			transform.Rotate(Vector3.up, -Time.deltaTime * 30f);
		}
		else
		{

		}
	}

	public void OnCharacterModelHover(CharacterModel m)
	{
		if (this == m)
		{
			StopCoroutine("IShrink");

			StopCoroutine("IExpand");

			StartCoroutine("IExpand");
		}
		else
		{
			StopCoroutine("IExpand");

			StopCoroutine("IShrink");

			StartCoroutine("IShrink");
		}
	}

	IEnumerator IExpand()
	{

		Vector3 expandScale = new Vector3(1.2f, 1.2f, 1.2f);

		while (transform.localScale != expandScale)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, expandScale, Time.deltaTime * 10f);
			yield return null;
		}
	}

	IEnumerator IShrink()
	{
		Vector3 target = Vector3.one * .2f;
		Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 90, 0));
		while (transform.localScale != target || transform.rotation != targetRotation)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * 10f);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
			yield return null;
		}

	}
}
