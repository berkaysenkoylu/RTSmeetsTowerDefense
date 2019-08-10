using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Zombie")
        {
            other.gameObject.GetComponent<Zombie>().GetDamaged(damage);
        }
    }
}
