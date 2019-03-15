using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    Inventory playerInventory;

    void Start()
    {
        playerInventory = new Inventory();

        

        /*
        foreach(KeyValuePair<Inventory.ResourceTypes, int> item in playerInventory.GetInventoryContent())
        {
            Debug.Log(item.Key + ": " + item.Value);
        }*/
    }

    void Update()
    {
        
    }
}
