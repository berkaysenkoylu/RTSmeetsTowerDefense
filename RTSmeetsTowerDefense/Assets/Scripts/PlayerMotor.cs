using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Vector3 destination;
    bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(agent.remainingDistance <= 0.5f)
        {
            isMoving = false;

            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isIdle", !isMoving);
        }
        else
        {
            isMoving = true;
        }
    }

    public void GoToLocation(Vector3 location)
    {
        NavMeshHit hit;

        destination = location;

        if(NavMesh.SamplePosition(destination, out hit, 5, NavMesh.GetAreaFromName("Ground")))
        {
            agent.SetDestination(destination);

            isMoving = true;

            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isIdle", !isMoving);
        }
    }
}
