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

	public void RegularAttack_Start(string _id)
	{
		if (EventManager.OnRegularAttackStart != null)
		{
			EventManager.OnRegularAttackStart(_id);
		}
	}

	public void RegularAttack_End(string _id)
	{
		if (EventManager.OnRegularAttackEnd != null)
		{
			EventManager.OnRegularAttackEnd(_id);
		}
	}

	public void GoblinAttack()
	{
		Enemy e = GetComponent<Enemy>();
		if (e != null)
		{
			e.Attack();
		}
	}

	public void OrcAttack()
	{
		Enemy e = GetComponent<Enemy>();
		if (e != null)
		{
			e.Attack();
		}
	}
}
