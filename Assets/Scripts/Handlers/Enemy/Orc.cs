using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : Enemy
{

	public override void Init ()
	{
		base.Init ();
		_inFireArea = false;
	}
}