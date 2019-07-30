using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTower : Tower
{
    public bool isBuilt = false;
    public float smoothTiming = 1f;
    public int woodCost = 10;

    private void Start()
    {
        Init();

        StartCoroutine("BuildingProcess");
    }

    public override void Init()
    {
        healthPoint = 50f;
        damageAmount = 5f;
        range = 5f;
    }

    public override IEnumerator BuildingProcess()
    {
        Vector3 finalLocation = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 refVelocity = Vector3.zero;

        while (!isBuilt)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalLocation, ref refVelocity, smoothTiming);

            if(Vector3.Distance(transform.position, finalLocation) <= 0.01f)
            {
                isBuilt = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        Debug.Log("Building has been completed!");
    }
}
