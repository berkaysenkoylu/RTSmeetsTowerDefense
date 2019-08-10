using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTower : Tower
{
    public int rockCost = 30;

    [Header("Misc")]
    public GameObject target;
    public Transform head;
    public Transform cannon;
    public GameObject bullet;

    float lastTimeFired;
    float fireRate = 2f;
    float damage;
    float range;
    float angle;

    public override void Init()
    {
        damage = damageAmount;
        range = maxRange;
    }

    void Start()
    {
        Init();

        StartCoroutine("BuildingProcess");
    }

    private void Update()
    {
        if (isBuilt)
            FollowTarget();
    }

    void FixedUpdate()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) > range)
        {
            target = null;
        }

        if(target && !target.activeSelf)
        {
            target = null;
        }

        if (isBuilt && target == null)
            target = GetTarget(range);

        if (target && Time.time - lastTimeFired >= fireRate && IsFacingTarget())
        {
            Fire();
        }
    }

    void FollowTarget()
    {
        if(target == null)
        {
            return;
        }

        // ===== Rotation of head section ===== //
        Vector3 direction = target.transform.position - head.position;

        direction.y = 0f;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        head.rotation = Quaternion.Slerp(head.rotation, lookRotation, 1f);
        // ===== Rotation of head section ===== //

        // ===== Rotation of cannon ===== //
        cannon.LookAt(target.transform);

        Vector3 eulerAngles = cannon.eulerAngles;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, 0f, 33f);

        cannon.eulerAngles = eulerAngles;
        // ===== Rotation of cannon ===== //
    }

    bool IsFacingTarget()
    {
        Vector3 dir = target.transform.position - cannon.position;

        return Vector3.Angle(dir, cannon.forward) <= 0.5f;
    }

    //void SetTarget()
    //{
    //    // Get all the targets in a certain range
    //    Vector3 sphereOrigin = new Vector3(transform.position.x, 0f, transform.position.z);
    //    Collider[] possibleTargets = Physics.OverlapSphere(sphereOrigin, range, zombieLayer);

    //    if (possibleTargets.Length <= 0f)
    //    {
    //        return;
    //    }

    //    // Find the closest one amongst them, and set the target to it
    //    target = GetClosestTarget(possibleTargets);
    //}

    //GameObject GetClosestTarget(Collider[] possibleTargetColliders)
    //{
    //    GameObject closestTarget = possibleTargetColliders[0].gameObject;

    //    float distance = Vector3.Distance(transform.position, possibleTargetColliders[0].transform.position);

    //    for (int i = 1; i < possibleTargetColliders.Length; i++)
    //    {
    //        float currDistance = Vector3.Distance(transform.position, possibleTargetColliders[i].transform.position);

    //        if (currDistance < distance && possibleTargetColliders[i].gameObject.activeSelf)
    //        {
    //            distance = currDistance;

    //            closestTarget = possibleTargetColliders[i].gameObject;
    //        }
    //    }

    //    return closestTarget;
    //}

    void Fire()
    {
        lastTimeFired = Time.time;

        GameObject cannonBall = Instantiate(bullet, muzzle.position, Quaternion.identity);

        cannonBall.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * cannonBall.GetComponent<CannonBall>().speed);
    }
}
