using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    Inventory playerInventory; // Inventory class reference

    void Awake()
    {
        // Creating an instance of Inventory class
        playerInventory = new Inventory();
    }

    // Function for incrementing the item amounts in inventory
    public void IncrementItemInInventory(string resourceName, int amount)
    {
        // Convert the name of the resource to 'ResourceTypes' enum, residing in Inventory
        Inventory.ResourceTypes resType = (Inventory.ResourceTypes)Enum.Parse(typeof(Inventory.ResourceTypes), resourceName.ToLower());

        // Call the related function residing in Inventory class to add the item corresponding to inputted arguments
        playerInventory.AddItemToInventory(resType, amount);
    }

    // Function to output the inventory class instance
    public Inventory GetInventory()
    {
        return playerInventory;
    }
}
