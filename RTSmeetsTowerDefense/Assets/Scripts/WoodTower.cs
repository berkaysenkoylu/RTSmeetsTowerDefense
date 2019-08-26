using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTower : Tower
{
    public int woodCost = 10;
    public GameObject projectile;

    [Header("Misc")]
    public GameObject target;

    float lastTimeFired;
    float fireRate = 1f;
    float damage; // Remove later (leave it maybe??)
    float range;
    AudioSource audioSource;

    private void Start()
    {
        Init();

        audioSource = GetComponent<AudioSource>();

        StartCoroutine("BuildingProcess");
    }

    public override void Init()
    {
        damage = damageAmount;
        range = maxRange;
    }

    private void Update()
    {
        if (isBuilt && target == null)
            target = GetTarget(range);
    }

    private void FixedUpdate()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) > range)
        {
            target = null;
        }

        if (target && !target.activeSelf)
        {
            target = null;
        }

        if (target && Time.time - lastTimeFired >= fireRate)
        {
            Fire();
        }
    }

    void Fire()
    {
        lastTimeFired = Time.time;

        GameObject bullet = Instantiate(projectile, muzzle.position, Quaternion.identity);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bullet.GetComponent<WoodProjectile>().SetDamage(damage);

        Vector3 direction = (target.transform.position - muzzle.position).normalized;

        bullet.transform.LookAt(target.transform.position);

        audioSource.Play();

        bulletRb.AddForce(direction * bullet.GetComponent<WoodProjectile>().speed);
    }
}
