using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour
{
    PlayerInventory playerInventory;
    Animator animator; // Reference for animator on the player
    NavMeshAgent agent; // Reference for navmeshagent component on the player
    Vector3 destination; // Reference for a location called 'destination'
    bool isMoving = false; // A flag to signify whether the player is moving or not
    bool isCollecting = false; // A flag to show if player is collecting resources
    bool isGoingToCollectResource = false; // A flag to show if player is going to collect the resource

    public GameObject targetResource; // Reference for target resource
    public AnimationClip[] animClips;
    public GameObject[] tools;

    void Start()
    {
        // Set up the references
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        // Check if the remaining distance of the navmesh agent is smaller than a certain threshold
        if(agent.remainingDistance <= 0.4f) // TODO: tweak this offset
        {
            // If so, then set the moving false. Since this implies that the destination has been reached
            isMoving = false;

            // Set the bools in animator to the corresponding values
            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isIdle", !isMoving);
        }

        // Check if player has target resource, and the distance between the target resource and the player is less than a certain value
        if (targetResource != null && Vector3.Distance(transform.position, targetResource.transform.position) <= 2.0f) // TODO: Tweak number
        {
            Debug.Log(Vector3.Distance(transform.position, targetResource.transform.position));
            // Form the direction vector
            Vector3 dir = (targetResource.transform.position - transform.position).normalized;

            // Set its y component 0, since we don't care about y axis
            dir.y = 0f;

            // Setup a quaternion: rotation
            Quaternion rotation = Quaternion.LookRotation(dir);

            // Slowly make character face the target resource
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2.0f);
            
            // Check if player is going to collect the resource, and if player is already collecting or not
            if (isGoingToCollectResource && !isCollecting)
            {
                // If satisfied, start the resource collection process
                StartCoroutine("CollectResource");
            }
        }
    }

    // Function for moving the character to a point, given by an argument called 'location'
    public void GoToLocation(Vector3 location)
    {
        // If in building mode, then moving is not allowed
        if (BuildManager.instance.isBuildingMode())
        {
            return;
        }

        // Form a navmeshhit variable
        NavMeshHit hit;

        // Check whether or not we have a navmesh within 3.0 br of the click point
        // This is to make sure, even if player doesn't click on a walkable area on navmesh,
        // if there is a walkable navmesh around(specified with the 3rd arg) the click location,
        // we can still move the player to that location with the utilization of the following:
        if (NavMesh.SamplePosition(location, out hit, 3f, NavMesh.GetAreaFromName("Ground")))
        {
            // Set the agent destination to that point
            agent.SetDestination(hit.position);

            // Set the isMoving flag to true
            isMoving = true;

            // Set the bools in animator correspondingly
            animator.SetBool("isWalking", isMoving);
            animator.SetBool("isIdle", !isMoving); 
        }
    }

    #region Resource Collection Related
    // Function for setting the target resource
    public void SetTargetResource(GameObject newTarget)
    {
        // Set isGoingToCollectResource to true (since player clicks on a resource)
        isGoingToCollectResource = true;

        // Set the target resource to the input newTarget gameobject 
        //(this is the resource to be collected, if player doesn't abort(by clicking elsewhere))
        targetResource = newTarget;
    }

    // Function for reseting the target resource: If player clicks on terrain,
    // or anything that is not a harvestable resource, this function is called
    public void ResetTargetResource()
    {
        // Set the target resource to null
        targetResource = null;

        // Set isGoingToCollectResource to false, since player aborts the resource collection process
        isGoingToCollectResource = false;
    }

    // Function to return if player is already collecting a resource or not
    public bool IsPlayerCollecting()
    {
        return isCollecting;
    }

    // Function to handle resource collection
    // TODO: Add animation transitions (WIP)
    public IEnumerator CollectResource()
    {
        Debug.Log("Collecting the resource");

        // Get the resource component of the harvested resource
        Resource harvestedResource = targetResource.GetComponent<Resource>();

        // Set is collecting true
        isCollecting = true;

        // TODO: (WIP) Handle correct animation transitions here, depending on what kind of resource is being collected
        switch (harvestedResource.resourceName)
        {
            case "Log":
                tools[0].SetActive(true);
                animator.SetTrigger("isChopping");
                yield return new WaitForSeconds(animClips[0].length);
                break;
            case "Rock":
                tools[1].SetActive(true);
                animator.SetTrigger("isMining");
                yield return new WaitForSeconds(animClips[1].length);
                break;
            default:
                Debug.Log("Wrong resource type!");
                break;
        }

        // Wait for a while
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Collected the resource: " + targetResource.tag);

        // Increment the amount of the associated resource in the inventory
        playerInventory.IncrementItemInInventory(harvestedResource.resourceName,
                                                 harvestedResource.resourceYield);

        // Having collected the resource, set isCollecting to false
        isCollecting = false;

        // Call the function 'ResetTargetResource', to reset the resource collection process
        ResetTargetResource();

        // Deactivate the tools
        foreach (GameObject tool in tools)
            tool.SetActive(false);
    }
    #endregion
}
