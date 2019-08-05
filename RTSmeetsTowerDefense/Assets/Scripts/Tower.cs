using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Attributes shared with all the types of towers
    public bool isBuilt = false;
    public float smoothTiming = 1f;
    public float damageAmount;
    public float maxRange;

    public virtual void Init()
    {

    }

    public virtual IEnumerator BuildingProcess()
    {
        Vector3 finalLocation = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 refVelocity = Vector3.zero;

        while (!isBuilt)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalLocation, ref refVelocity, smoothTiming);

            if (Vector3.Distance(transform.position, finalLocation) <= 0.01f)
            {
                isBuilt = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Building has been completed!");
    }
}
