using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Attributes shared with all the types of towers
    [Header("Features")]
    public bool isBuilt = false;
    public float smoothTiming = 1f;
    public float damageAmount;
    public float maxRange;
    public LayerMask zombieLayer;
    public Transform muzzle;
    public GameObject buildingEffect;

    public virtual void Init()
    {

    }

    public virtual IEnumerator BuildingProcess()
    {
        Vector3 finalLocation = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 refVelocity = Vector3.zero;
        GameObject buildEffect = Instantiate(buildingEffect, finalLocation, Quaternion.identity);

        while (!isBuilt)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalLocation, ref refVelocity, smoothTiming);

            if (Vector3.Distance(transform.position, finalLocation) <= 0.01f)
            {
                isBuilt = true;
            }

            yield return null;
        }

        Destroy(buildEffect);

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Building has been completed!");
    }

    public virtual GameObject GetTarget(float range)
    {
        GameObject target = null;

        // Get all the targets in a certain range
        Vector3 sphereOrigin = new Vector3(transform.position.x, 0f, transform.position.z);
        Collider[] possibleTargets = Physics.OverlapSphere(sphereOrigin, range, zombieLayer);

        if (possibleTargets.Length <= 0f)
        {
            return target;
        }

        // Find the closest one amongst them, and set the target to it
        target = GetClosestTarget(possibleTargets);

        return target;
    }

    GameObject GetClosestTarget(Collider[] possibleTargetColliders)
    {
        GameObject closestTarget = possibleTargetColliders[0].gameObject;

        float distance = Vector3.Distance(transform.position, possibleTargetColliders[0].transform.position);

        for (int i = 1; i < possibleTargetColliders.Length; i++)
        {
            float currDistance = Vector3.Distance(transform.position, possibleTargetColliders[i].transform.position);

            if (currDistance < distance && possibleTargetColliders[i].gameObject.activeSelf)
            {
                distance = currDistance;

                closestTarget = possibleTargetColliders[i].gameObject;
            }
        }

        return closestTarget;
    }
}
