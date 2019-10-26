using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy
{

	//public ParticleSystem fireFX;
	public override void Init()
	{
		base.Init();
		_inFireArea = false;
	}
}
