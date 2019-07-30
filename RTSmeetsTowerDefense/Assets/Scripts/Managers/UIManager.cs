using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite[] resourceIcons; // An array to keep the sprites to use as icons of resources in individual resource GUI
    public GameObject resource; // Prefab that represents resource: Icon (raw image): Amount (text)

    public Slider dayTimeSlider;
    public Sprite[] dayTimeSprites;
    public Image background;
    public Image fillImage;

    List<GameObject> resourceGUIContainer; // List to hold all the resource GUI prefabs (to be used while updating the values)
    PlayerInventory playerInventoryScript; // Reference for player inventory script
    Inventory playerInventory; // Reference for player inventory

    void Start()
    {
        // Start listening to an event called: "Resource amount changed", which 
        // will be emitted from PlayerMotor.cs after having harvested a resource
        EventManager.StartListening("Resource amount changed", UpdateResourceGUI);

        // Get the player inventory script
        playerInventoryScript = FindObjectOfType<PlayerInventory>();

        // Get the player inventory
        playerInventory = playerInventoryScript.GetInventory();

        // Initialize the list: resourceGUIContainer
        resourceGUIContainer = new List<GameObject>();

        // Call the function InitializeResourcesUI() to create the resource GUI
        InitializeResourcesUI();

        // TODO: Just tryin'
        Sun.whatTimeIsIt += UpdateDayTimeSlider;
    }

    // Function for initializing the resource GUI
    void InitializeResourcesUI()
    {
        // Iterate each Inventory.ResourceTypes, int pair in the dictionary residing in player inventory,
        // and create a resource GUI for each one of them
        foreach (KeyValuePair<Inventory.ResourceTypes, int> resource in playerInventory.GetInventoryContent())
        {
            CreateResourceGUI(resource.Key, resource.Value);
        }
    }

    // Function to create individual resource GUI element
    void CreateResourceGUI(Inventory.ResourceTypes type, int amount)
    {
        // Instantiate the prefab
        GameObject newResource = Instantiate(resource);

        // Set its parent to this transform
        newResource.transform.SetParent(transform);

        // Change its name. Using enum to string would be a good idea here
        // We can then reverse it to get the 'Inventory.ResourceTypes' enum if need be
        newResource.name = type.ToString();

        // Depending on the resource type, put appropriate icon image, and corresponding amount
        switch (type)
        {
            case Inventory.ResourceTypes.log:
                newResource.GetComponentInChildren<RawImage>().texture = resourceIcons[0].texture;
                newResource.GetComponentInChildren<Text>().text = amount.ToString();
                break;
            case Inventory.ResourceTypes.rock:
                newResource.GetComponentInChildren<RawImage>().texture = resourceIcons[1].texture;
                newResource.GetComponentInChildren<Text>().text = amount.ToString();
                break;
            default:
                break;
        }

        // Add this newly created resource GUI element to resourceGUIContainer list
        // We can then iterate through this list to update the resource amounts
        resourceGUIContainer.Add(newResource);
    }

    // Function for updating the resource GUI elements. This function will be called
    //  when 'Resource amount changed' event is triggered.
    void UpdateResourceGUI()
    {
        // Form a dictionary, whose key-value pairs are 'Inventory.ResourceTypes' and 'int'
        Dictionary<Inventory.ResourceTypes, int> inventoryContents = new Dictionary<Inventory.ResourceTypes, int>();

        // Populate the inventoryContents dictionary by calling the GetInventoryContent() method from PlayerInventory.cs
        inventoryContents = playerInventory.GetInventoryContent();

        // Iterate through resourceGUIContainer list
        foreach (GameObject resGUI in resourceGUIContainer)
        {
            // Convert resourceGUI element's name to Inventory.ResourceTypes enum type
            Inventory.ResourceTypes type = (Inventory.ResourceTypes)Enum.Parse(typeof(Inventory.ResourceTypes), resGUI.name);

            // Update the resourceGUI element's amount
            resGUI.GetComponentInChildren<Text>().text = inventoryContents[type].ToString();
        }
    }

    void UpdateDayTimeSlider(float dayTimePercentage)
    {
        if(dayTimePercentage <= 0.5f)
        {
            dayTimeSlider.direction = Slider.Direction.LeftToRight;
            dayTimeSlider.value = dayTimePercentage;

            // Change slider sprites
            background.sprite = dayTimeSprites[0];
            fillImage.sprite = dayTimeSprites[1];
        }
        else
        {
            dayTimeSlider.direction = Slider.Direction.RightToLeft;
            dayTimeSlider.value = dayTimePercentage - 0.5f;

            // Change slider sprites
            background.sprite = dayTimeSprites[1];
            fillImage.sprite = dayTimeSprites[0];
        }
    }
}
