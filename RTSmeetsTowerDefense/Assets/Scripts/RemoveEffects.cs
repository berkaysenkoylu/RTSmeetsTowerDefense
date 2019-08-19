using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveEffects : MonoBehaviour
{
    public float effectLifeTime = 1.5f;

    void Start()
    {
        Destroy(gameObject, effectLifeTime);
    }
}
