using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour {

    public float speed;
    public float damage;
    public float health;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    public void Move()
    {
        if(Input.GetKey("w"))
        {
            transform.Translate(0, 0, speed);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(-speed, 0, 0);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(0, 0, -speed);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(speed, 0, 0);
        }
    }
}
