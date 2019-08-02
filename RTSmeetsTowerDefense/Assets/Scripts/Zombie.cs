using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class Zombie : MonoBehaviour
{
    public float healthPoint = 100f;
    public float attackRange = 4f;
    public GameObject mainTarget;
    public LayerMask _mask;

    [SerializeField]
    Vector3 targetPosition;
    NavMeshAgent zombieAgent;
    ZombieAI zombieAI;

    Node zombieBT;

    private void Awake()
    {
        zombieAI = GetComponent<ZombieAI>();
    }

    void Start()
    {
        zombieAgent = GetComponent<NavMeshAgent>();

        zombieBT = zombieAI.CreateBehaviorTree(this, mainTarget, _mask);
    }

    void Update()
    {
        zombieBT.Behave();

        if (Input.GetKeyDown(KeyCode.E))
        {
            GetDamaged(10f);
        }
    }

    public void SetMainTarget(GameObject target)
    {
        mainTarget = target;
    }

    public GameObject GetMainTarget()
    {
        return mainTarget;
    }

    public void SetTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
    }

    public bool TargetWithinDistance()
    {
        return Vector3.Distance(transform.position, mainTarget.transform.position) <= attackRange;
    }

    public void MoveToDestination()
    {
        Vector3 desiredLocation = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        if (Vector3.Distance(transform.position, desiredLocation) <= attackRange)
        {
            zombieAgent.isStopped = true;
        }

        zombieAgent.SetDestination(desiredLocation);
    }







    void GetDamaged(float amount)
    {
        healthPoint -= amount;
    }

    public float getHealthPoint()
    {
        return healthPoint;
    }
}
