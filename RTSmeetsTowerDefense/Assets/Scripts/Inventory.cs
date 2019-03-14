using System.Collections;
using System;
using System.Collections.Generic;

public class Inventory
{
    public enum ResourceTypes { log, rock }

    Dictionary<ResourceTypes, int> inventoryContent = new Dictionary<ResourceTypes, int>();

    public Inventory()
    {
        foreach (ResourceTypes resType in Enum.GetValues(typeof(ResourceTypes)))
        {
            inventoryContent.Add(resType, 0);
        }
    }

    public Dictionary<ResourceTypes, int> GetInventoryContent()
    {
        return inventoryContent;
    }

    public void AddItemToInventory(ResourceTypes type, int amount)
    {
        inventoryContent[type] += amount;
    }
}
