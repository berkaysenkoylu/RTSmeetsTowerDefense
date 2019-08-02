﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager buildManager;
    public Transform towerContainer;
    public GameObject[] towers;
    public GameObject[] ghostTowers;

    bool isBuildMode = false;

    // TODO: Add more variant of towers later and their corresponding ghost entities

    public static BuildManager instance
    {
        get
        {
            if (!buildManager)
            {
                buildManager = FindObjectOfType(typeof(BuildManager)) as BuildManager;

                if (!buildManager)
                {
                    Debug.LogError("There needs to be one active BuildManger script on a GameObject in your scene.");
                }
            }

            return buildManager;
        }
    }

    void Start()
    {
        EventManager.StartListening("AbortBuild", AbortBuilding);

        GhostTower.whereToBuild += BuildTower;
    }

    void Update()
    {

    }

    public void InitiateBuilding(string towerType)
    {
        if (isBuildMode)
        {
            return;
        }

        // Setting the building mode
        isBuildMode = true;

        switch (towerType)
        {
            case "wood":
                ghostTowers[0].SetActive(true);
                break;
        }
    }

    void BuildTower(Vector3 position, string towerType)
    {
        switch (towerType)
        {
            case "WoodGhost":
                position = new Vector3(position.x, -4.0f, position.z);
                GameObject newWoodTower = Instantiate(towers[0], position, Quaternion.identity);
                newWoodTower.transform.SetParent(towerContainer);
                break;
            default:
                Debug.Log("Oops something went wrong! Check your tags and shit man....");
                break;
        }
        
        isBuildMode = false;
    }

    void AbortBuilding()
    {
        isBuildMode = false;
    }

    public bool isBuildingMode()
    {
        return isBuildMode;
    }
}