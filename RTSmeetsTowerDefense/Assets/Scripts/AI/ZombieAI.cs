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

    public Node CreateBehaviorTree(Zombie zombie, GameObject mainTarget, LayerMask _mask)
    {
        Selector deathSequence = new Selector("Death Sequence", new ShouldIDie(zombie), new Die(zombie));

        Sequence targetSelectionSequence = new Sequence("Do I have a target?", new HaveTarget(zombie, _mask, zombie.GetMainTarget()), new SetDestination(zombie));

        Sequence moveSequence = new Sequence("Move Sequence", new Inverter(new CheckDistance(zombie)), new MoveTowardsEnemy(zombie));

        Sequence attackSequence = new Sequence("Attack Sequence", new ReadyToAttack(zombie));

        Sequence treeRoot = new Sequence("Root behaviour", deathSequence, targetSelectionSequence, moveSequence, attackSequence);

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
            // TODO: Implement death mechanism: To destroy, to deactivate, or ... something else?
            Debug.Log("Zomboi is dead");
            Destroy(self.gameObject);

            // To break the whole sequence
            return NodeStatus.FAILURE;
        }

        public override void OnReset() { }
    }

    public class HaveTarget : Leaf
    {
        Zombie self;
        GameObject target;
        LayerMask attackMask;

        public HaveTarget(Zombie own, LayerMask _mask, GameObject defaultTarget)
        {
            self = own;
            attackMask = _mask;
            target = defaultTarget;
        }

        public override NodeStatus OnBehave()
        {
            // Check if there are potential targets in the zombie's surroundings (such as player, towers, ...etc)
            Collider[] targetColliders = Physics.OverlapSphere(self.gameObject.transform.position, 10f, attackMask);

            GameObject closestTarget = GetClosestTarget(targetColliders);

            if(closestTarget != null)
            {
                target = closestTarget;
            }

            if (target != null)
            {
                self.SetMainTarget(target);

                return NodeStatus.SUCCESS;
            }
            else
            {
                return NodeStatus.FAILURE;
            }
        }

        GameObject GetClosestTarget(Collider[] potentialTargets)
        {
            float distance = 999f;
            GameObject closestTarget = null;

            foreach(Collider potentialTarget in potentialTargets)
            {
                float currDistance = Vector3.Distance(self.transform.position, potentialTarget.gameObject.transform.position);

                if (currDistance < distance)
                {
                    closestTarget = potentialTarget.gameObject;
                    distance = currDistance;
                }
            }

            return closestTarget;
        }

        public override void OnReset() { }
    }

    public class SetDestination : Leaf
    {
        Zombie self;

        public SetDestination(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            Debug.Log("Setting destination");

            GameObject target = self.GetMainTarget();

            if (target != null)
            {
                self.SetTargetPosition(target.transform.position);

                return NodeStatus.SUCCESS;
            }

            return NodeStatus.FAILURE;
        }

        public override void OnReset() { }
    }

    // TODO: Stuff that is below is likely to be changed...

    public class CheckDistance : Leaf
    {
        Zombie self;

        public CheckDistance(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            if (self.TargetWithinDistance())
            {
                Debug.Log("Target is within distance");
                return NodeStatus.SUCCESS;
            }
            else
            {
                Debug.Log("Target is not within distance");
                return NodeStatus.FAILURE;
            }
        }

        public override void OnReset() { }
    }

    public class MoveTowardsEnemy : Leaf
    {
        Zombie self;

        public MoveTowardsEnemy(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            float distance = Vector3.Distance(self.gameObject.transform.position, self.GetMainTarget().transform.position);

            if (distance > 4f)
            {
                self.MoveToDestination();
            }

            if(distance <= 4f)
            {
                return NodeStatus.SUCCESS;
            }
            else
            {
                return NodeStatus.RUNNING;
            }
        }

        public override void OnReset() { }
    }

    public class ReadyToAttack : Leaf
    {
        Zombie self;

        public ReadyToAttack(Zombie own)
        {
            self = own;
        }

        public override NodeStatus OnBehave()
        {
            Debug.Log("Zombie is ready to attack");

            return NodeStatus.SUCCESS;
        }

        public override void OnReset() { }
    }
    // ============================================= //
}
