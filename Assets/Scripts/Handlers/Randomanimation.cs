using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomanimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStateMachineEnter(Animator animator, int StateMachinePathMash)
    {
        animator.SetInteger("AttackID", Random.Range(0, 4));
    }
}
