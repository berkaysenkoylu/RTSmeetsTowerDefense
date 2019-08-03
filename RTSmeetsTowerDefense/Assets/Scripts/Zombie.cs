using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class Zombie : MonoBehaviour
{
    public float healthPoint = 100f;
    public float attackRange = 5f;
    public GameObject mainTarget;
    public float attackPower = 10f;
    public GameObject healthBar;

    Vector3 targetPosition;
    NavMeshAgent zombieAgent;
    bool canAttack = true;
    float attackRate = 1.5f;

    ZombieAI zombieAI;
    Node zombieBT;

    public delegate void ZombieDamageDealt(float damageAmount);
    public static event ZombieDamageDealt DealDamage;

    private void Awake()
    {
        zombieAI = GetComponent<ZombieAI>();
    }

    void Start()
    {
        zombieAgent = GetComponent<NavMeshAgent>();

        zombieBT = zombieAI.CreateBehaviorTree(this);
    }

    void Update()
    {
        zombieBT.Behave();

        if (!canAttack)
        {
            attackRate -= Time.deltaTime;
        }

        if (attackRate <= 0f)
        {
            attackRate = 1.5f;
            canAttack = true;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            GetDamaged(10f);
        }

        healthBar.transform.LookAt(Camera.main.transform);
    }

    public GameObject GetMainTarget()
    {
        return mainTarget;
    }

    public bool getCanAttack()
    {
        return canAttack;
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
        else
        {
            zombieAgent.SetDestination(desiredLocation);
        }
    }

    public void MeleeAttackTarget()
    {
        // Face the target first
        Vector3 dir = (mainTarget.transform.position - transform.position).normalized;

        dir.y = 0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 1.5f);

        // Attack the target
        if (canAttack)
        {
            Debug.Log("SHRED!!!");

            DealDamage(attackPower);

            canAttack = false;
        }
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
