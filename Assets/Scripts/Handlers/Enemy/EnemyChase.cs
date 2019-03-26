using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour {

    public Transform player;
    private NavMeshAgent agent;
    private float distance, timer = 0;
    private Vector3 distanceVector;
    public Material color;
    // Use agent for initialization
    void Start ()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
        agent.SetDestination(player.position);
        if (checkDistance() > 5)
        {
            agent.isStopped = false;
            color.color = Color.green;
        }
        else
        {
            agent.isStopped = true;
            attack();
        }
    }

    public float checkDistance()
    {
        distanceVector.x = agent.transform.position.x - player.position.x;
        distanceVector.y = agent.transform.position.y - player.position.y;
        distanceVector.z = agent.transform.position.z - player.position.z;

        distance = distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z;
        return distance;
    }

    public void attack()
    {
        if (timer <= 0)
        {
            color.color = Color.red;
            Debug.Log("working");
            if (timer <= -.1f)
            {
                timer = 3;
                color.color = Color.green;
            }
        }
        timer -= Time.deltaTime;
    }
}
