using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour {


	public void CharacterOne_Special()
	{
		if (EventManager.OnSpecialAttackHit != null)
		{
			EventManager.OnSpecialAttackHit("character_one");
		}
	}

	public void HammerSpecial()
	{
		if (EventManager.OnSpecialAttackHit != null)
		{
			EventManager.OnSpecialAttackHit("character_two");
		}
	}
}
