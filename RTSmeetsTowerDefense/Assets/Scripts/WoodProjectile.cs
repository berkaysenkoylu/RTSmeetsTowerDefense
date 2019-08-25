using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    public GameObject bloodSplatterEffect;

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
        if (other.gameObject.tag == "Zombie")
        {
            Vector3 effectSpawnPos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            GameObject bloodEffect = Instantiate(bloodSplatterEffect, effectSpawnPos, Quaternion.identity);

            bloodEffect.transform.forward = transform.forward;

            other.gameObject.GetComponent<Zombie>().GetDamaged(damage);
        }
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }
}
