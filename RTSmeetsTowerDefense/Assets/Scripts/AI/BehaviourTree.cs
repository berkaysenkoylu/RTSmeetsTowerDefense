using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeStatus
    {
        NONE,
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public abstract class Node
    {
        public bool starting = true;
        public int ticks = 0;

        public virtual NodeStatus Behave()
        {
            NodeStatus status = OnBehave();

            ticks++;
            starting = false;

            if (status != NodeStatus.RUNNING)
                Reset();

            return status;
        }

        public abstract NodeStatus OnBehave();

        public void Reset()
        {
            starting = true;
            ticks = 0;
            OnReset();
        }

        public abstract void OnReset();
    }

    public abstract class Composite : Node
    {
        protected List<Node> children = new List<Node>();
        public string compositeName;

        public Composite(string name, params Node[] nodes)
        {
            this.compositeName = name;
            this.children.AddRange(nodes);
        }

        public override NodeStatus Behave()
        {
            NodeStatus status = base.Behave();

            return status;
        }
    }

    public abstract class Leaf : Node { }

    public abstract class Decorator : Node
    {
        protected Node child;

        public Decorator(Node node)
        {
            this.child = node;
        }
    }

    public class Inverter : Decorator
    {
        public Inverter(Node child) : base(child) { }

        public override NodeStatus OnBehave()
        {
            switch (child.Behave())
            {
                case NodeStatus.RUNNING:
                    return NodeStatus.RUNNING;
                case NodeStatus.FAILURE:
                    return NodeStatus.SUCCESS;
                case NodeStatus.SUCCESS:
                    return NodeStatus.FAILURE;
            }

            Debug.Log("We shouldn't be here, something went wrong: ");
            return NodeStatus.FAILURE;
        }

        public override void OnReset() { }
    }

    public class Sequence : Composite
    {
        int currentChild = 0;

        public Sequence(string compositeName, params Node[] nodes) : base(compositeName, nodes) { }

        public override NodeStatus OnBehave()
        {
            NodeStatus status = children[currentChild].Behave();

            switch (status)
            {
                case NodeStatus.SUCCESS:
                    currentChild++;
                    break;
                case NodeStatus.FAILURE:
                    return NodeStatus.FAILURE;
            }

            if (currentChild >= children.Count)
            {
                return NodeStatus.SUCCESS;
            }
            else if (status == NodeStatus.SUCCESS)
            {
                return OnBehave();
            }

            return NodeStatus.RUNNING;
        }

        public override void OnReset()
        {
            currentChild = 0;
            foreach (Node child in children)
            {
                child.Reset();
            }
        }
    }

    public class Selector : Composite
    {
        int currentChildCount = 0;

        public Selector(string compositeName, params Node[] nodes) : base(compositeName, nodes) { }

        public override NodeStatus OnBehave()
        {
            if (currentChildCount >= children.Count)
                return NodeStatus.FAILURE;

            NodeStatus status = children[currentChildCount].Behave();

            switch (status)
            {
                case NodeStatus.SUCCESS:
                    return NodeStatus.SUCCESS;

                case NodeStatus.FAILURE:
                    currentChildCount++;
                    return OnBehave();
            }

            return NodeStatus.RUNNING;
        }

        public override void OnReset()
        {
            currentChildCount = 0;

            foreach (Node childNode in children)
            {
                childNode.Reset();
            }
        }
    }
}
