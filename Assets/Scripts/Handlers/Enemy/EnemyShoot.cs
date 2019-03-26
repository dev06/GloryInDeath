using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{

    public Transform player;
    public Material color;
    private NavMeshAgent agent;
    private float distance, timer = 0;
    private Vector3 distanceVector;
    // Use agent for initialization
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
        if (checkDistance() > 100)
        {
            agent.isStopped = false;
            color.color = Color.black;
        }
        else
        {
            agent.isStopped = true;
            shoot();
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

    public void shoot()
    {
        if(timer <= 0)
        {
            color.color = Color.red;
            Debug.Log("working");
            if(timer<= -.1f)
            {
                timer = 3;
                color.color = Color.black;
            }
        }
            timer -= Time.deltaTime;
    }
}
