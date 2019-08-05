using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTower : Tower
{
    public int rockCost = 30;

    [SerializeField]
    float damage;
    [SerializeField]
    float range;

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
}
