using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayText : MonoBehaviour
{

	public bool enableRotation = true;
	private List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();
	private List<Animation> _textAnimations = new List<Animation>();
	private int index;
	void Start()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			_texts.Add(transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
		}

		for (int i = 0; i < transform.childCount; i++)
		{
			_textAnimations.Add(transform.GetChild(i).GetChild(0).GetComponent<Animation>());
		}
	}

	public void Show(string _text, Color c)
	{
		if (_textAnimations[index].isPlaying == false)
		{
			_textAnimations[index].Play();
			if (enableRotation)
			{
				transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-15f, 15f)));
			}
		}
		_texts[index].text = _text;
		_texts[index].color = c;
		index++;
		if (index > transform.childCount - 1)
		{
			index = 0;
		}
	}
}
