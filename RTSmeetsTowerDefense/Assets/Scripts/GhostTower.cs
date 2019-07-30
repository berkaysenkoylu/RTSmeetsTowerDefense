using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTower : MonoBehaviour
{
    public LayerMask layerMask;
    public Color[] colors;
    public GameObject player;
    public Vector2 minMaxDistance = new Vector2(2f, 5f);
    public float maxPerimeterRadius = 5.0f;

    Material ghostTowerMaterial;
    BuildManager buildManager;

    // Event handling
    public delegate void PositionToBuildDeclaration(Vector3 position, string towerType);
    public static event PositionToBuildDeclaration whereToBuild;

    void Start()
    {
        ghostTowerMaterial = GetComponent<MeshRenderer>().material;

        buildManager = FindObjectOfType<BuildManager>();
    }

    void Update()
    {
        //Debug.Log(Physics.OverlapSphere(transform.position, maxPerimeterRadius).Length);
        // While active, follow the position of the mouse
        FollowMousePosition();

        // Cancel the building process
        if (Input.GetMouseButtonDown(1))
        {
            // Trigger an event stating that building process should be aborted. BuildManager.cs is listening
            EventManager.TriggerEvent("AbortBuild");

            // Reset the ghost's position
            transform.position = new Vector3(0f, -10f, 0f);

            // Disable the ghost gameobject
            gameObject.SetActive(false);
        }

        // TODO: Add some conditions to be able to build the tower
        // Change the color of the tower to give player some feedback
        if (CanBuildTower())
        {
            ghostTowerMaterial.color = colors[0];

            // If left mouse button is clicked while ghost is active, then build the tower at that position
            if (Input.GetMouseButton(0))
            {
                whereToBuild(transform.position, tag);

                // Subtract the resource from player's inventory
                player.GetComponent<PlayerInventory>().DecrementItemInInventory("log", buildManager.towers[0].GetComponent<WoodTower>().woodCost);

                // Reset the ghost's position
                transform.position = new Vector3(0f, -10f, 0f);

                // Disable the ghost gameobject
                gameObject.SetActive(false);
            }
        } else
        {
            ghostTowerMaterial.color = colors[1];
        }
    }

    void FollowMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerMask))
        {
            Vector3 targetPosition = new Vector3(hit.point.x, 0f, hit.point.z);

            transform.position = targetPosition;
        }
    }

    bool CanBuildTower()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        
        // Check distance to player
        if (distance < minMaxDistance.x || distance > minMaxDistance.y)
        {
            return false;
        }

        Collider[] perimeterObjects = Physics.OverlapSphere(transform.position, maxPerimeterRadius);

        // Check if there is any other tower or resources within a certain perimeter
        if (perimeterObjects.Length > 2)
        {
            return false;
        }

        // Check if there is enough resource
        if (tag == "WoodGhost")
        {
            Inventory.ResourceTypes type = (Inventory.ResourceTypes)Enum.Parse(typeof(Inventory.ResourceTypes), "log");

            if (player.GetComponent<PlayerInventory>().GetInventory().GetInventoryContent()[type] < buildManager.towers[0].GetComponent<WoodTower>().woodCost)
            {
                return false;
            }
        }
        
        // If all the conditions are met, then signal the ability for tower to be built
        return true;
    }
}
