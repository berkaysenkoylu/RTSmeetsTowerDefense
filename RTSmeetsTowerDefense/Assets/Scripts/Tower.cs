using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float healthPoint;
    public float damageAmount;
    public float range;

    public virtual void Init()
    {
        healthPoint = 100f;
        damageAmount = 10f;
        range = 7f;
    }

    public virtual IEnumerator BuildingProcess()
    {
        yield return null;
    }
}
