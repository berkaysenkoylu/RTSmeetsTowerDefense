using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ZombieAI : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public Node CreateBehaviorTree(Zombie zombie)
    {
        Selector deathSequence = new Selector("Death Sequence", new ShouldIDie(zombie), new Die(zombie));

        // GetReadyToAttack can cause jittering; remove it if need be
        Sequence attackEnemy = new Sequence("Attack Enemy", new GetMoving(zombie), new GetReadyToAttack(zombie), new MeleeAttack(zombie));

        Sequence treeRoot = new Sequence("Root behaviour", deathSequence, attackEnemy);

        return treeRoot;
    }

    // ================= LEAF Nodes ================ //
    public class ShouldIDie : Leaf
    {
        Zombie self;

        public ShouldIDie(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            if(self.getHealthPoint() <= 0f)
            {
                return NodeStatus.FAILURE;
            }
            
            return NodeStatus.SUCCESS;
        }

        public override void OnReset() { }
    }

    public class Die : Leaf
    {
        Zombie self;

        public Die(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            // Zombie dies
            self.InvokeDeathEvent();

            // To break the whole sequence
            return NodeStatus.FAILURE;
        }

        public override void OnReset() { }
    }

    public class GetMoving : Leaf
    {
        Zombie self;

        public GetMoving(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            if (self.getHealthPoint() <= 0f)
            {
                return NodeStatus.FAILURE;
            }

            if (self.TargetWithinDistance())
            {
                return NodeStatus.SUCCESS;
            }
            else
            {
                Vector3 targetPosition = self.GetMainTarget().transform.position;

                self.SetTargetPosition(targetPosition);

                self.MoveToDestination();

                return NodeStatus.RUNNING;
            }
        }

        public override void OnReset() { }
    }

    public class GetReadyToAttack : Leaf
    {
        Zombie self;

        public GetReadyToAttack(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            if (self.getHealthPoint() <= 0f)
            {
                return NodeStatus.FAILURE;
            }

            if (!self.getCanAttack())
            {
                //Debug.Log("Preparing to attack");
                return NodeStatus.RUNNING;
            } 
            else
            {
                return NodeStatus.SUCCESS;
            }
        }

        public override void OnReset() { }
    }

    public class MeleeAttack : Leaf
    {
        Zombie self;

        public MeleeAttack(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            if (self.getHealthPoint() <= 0f)
            {
                return NodeStatus.FAILURE;
            }

            if (self.TargetWithinDistance())
            {
                self.MeleeAttackTarget();

                return NodeStatus.SUCCESS;
            }
            else
            {
                return NodeStatus.FAILURE;
            }
        }

        public override void OnReset() { }
    }

    // ============================================= //
}
