using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float splashRadius = 5f;
    public float damage = 40f;
    public LayerMask zombieMask;
    public float speed = 10f;

    float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie" || collision.gameObject.layer == 9)
        {
            SplashDamage(collision.GetContact(0).point);
        }
    }

    void SplashDamage(Vector3 explosionOrigin)
    {
        // Get the potential victims
        Collider[] victims = Physics.OverlapSphere(explosionOrigin, splashRadius, zombieMask);

        // Make it so that damage is the highest at the explosion origin
        float damageAmount = damage / splashRadius;

        foreach(Collider zombie in victims)
        {
            float particularDamage = damage - (damageAmount * Vector3.Distance(explosionOrigin, zombie.transform.position));

            particularDamage = Mathf.Clamp(particularDamage, 0f, damage);

            // Apply damage to the victim
            zombie.GetComponent<Zombie>().GetDamaged(particularDamage);
        }

        // Destroy the cannon ball
        Destroy(gameObject);
    }
}
