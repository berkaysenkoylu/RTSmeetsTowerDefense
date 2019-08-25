using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsUIController : MonoBehaviour
{
    public Button toggleButton;
    public Text buttonText;
    public Color[] buttonColors;
    public GameObject tooltip;

    Animator animController;
    bool isPanelOpen = false;
    string openText = "Open Building Panel";
    string closeText = "Close Building Panel";
    Image buttonBackground;

    void Start()
    {
        animController = GetComponent<Animator>();
        buttonBackground = toggleButton.GetComponent<Image>();
    }

    void Update()
    {
        
    }

    public void ToggleBuildingPanel()
    {
        isPanelOpen = !isPanelOpen;

        animController.SetBool("isPanelOpen", isPanelOpen);

        if (isPanelOpen)
        {
            buttonText.text = closeText;
            buttonBackground.color = buttonColors[0];
        }
        else
        {
            buttonText.text = openText;
            buttonBackground.color = buttonColors[1];
        }
    }

    // TODO: Add tooltips that follow mouse while hovering on building buttons
    public void EnableTooltip(string type)
    {
        tooltip.SetActive(true);

        tooltip.GetComponent<Tooltip>().InitializeTooltip(type);
    }

    public void DisableTooltip()
    {
        tooltip.SetActive(false);
    }
}
