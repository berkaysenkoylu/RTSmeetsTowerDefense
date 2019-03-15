using System.Collections;
using System;
using System.Collections.Generic;

public class Inventory
{
    public enum ResourceTypes { log, rock } // Using enum to represent the types of resources

    Dictionary<ResourceTypes, int> inventoryContent = new Dictionary<ResourceTypes, int>(); // Dictionary to hold the inventory contents

    public Inventory()
    {
        // Initializing the inventory: Adding every specified 
        // resource type and setting their value to 0
        InitializeInventory();
    }

    // Function to output the inventory content
    public Dictionary<ResourceTypes, int> GetInventoryContent()
    {
        return inventoryContent;
    }

    // Function to increment the given amount of the specified resource type residing in the inventory
    public void AddItemToInventory(ResourceTypes type, int amount)
    {
        inventoryContent[type] += amount;
    }

    // Function to initialize the inventory
    void InitializeInventory()
    {
        // Creating items, corresponding to the specified 
        // resource types and setting their value to 0
        foreach (ResourceTypes resType in Enum.GetValues(typeof(ResourceTypes)))
        {
            inventoryContent.Add(resType, 0);
        }
    }
}
