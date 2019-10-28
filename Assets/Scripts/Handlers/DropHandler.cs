using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropProbs
{
	public static float HEALTH_POTION = .05f;
}
public class DropHandler : MonoBehaviour
{
	public static DropHandler Instance;
	public List<HealthPotion> potions = new List<HealthPotion>();
	private int _index;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start()
	{
		foreach (Transform t in transform)
		{
			potions.Add(t.GetComponent<HealthPotion>());
		}
	}

	public void Drop(Vector3 _position)
	{
		if (potions[_index].active == false)
		{
			potions[_index].Drop(_position);
		}
		_index++;
		if (_index > potions.Count - 1)
		{
			_index = 0;
		}
	}

}
