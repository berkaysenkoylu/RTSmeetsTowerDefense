using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    Inventory playerInventory;

    void Start()
    {
        playerInventory = new Inventory();
    }

    void Update()
    {
        
    }

    public void IncrementItemInInventory(string resourceName, int amount)
    {
        // Convert the name of the resource to 'ResourceTypes' enum, residing in Inventory
        Inventory.ResourceTypes resType = (Inventory.ResourceTypes)System.Enum.Parse(typeof(Inventory.ResourceTypes), resourceName.ToLower());

        playerInventory.AddItemToInventory(resType, amount);

        foreach(KeyValuePair<Inventory.ResourceTypes, int> item in playerInventory.GetInventoryContent())
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }
}
