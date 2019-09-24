using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golem : MonoBehaviour
{
	Animator _anim;
	void Start()
	{
		_anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_anim.SetTrigger("Attack");
			_anim.SetInteger("attackIndex", Random.Range(0, 4));
		}


	}
}
