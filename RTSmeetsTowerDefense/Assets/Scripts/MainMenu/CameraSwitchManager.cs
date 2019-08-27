using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchManager : MonoBehaviour
{
    public Camera[] cameras;
    public float cameraActivationDuration = 3f;
    public Animator animController;

    Camera currentActiveCamera;
    float timer = 0f;
    int index = 0;

    void Start()
    {
        animController.SetTrigger("fadeIn");
        // Current active camera
        currentActiveCamera = cameras[index];
        if (!currentActiveCamera.enabled)
        {
            currentActiveCamera.enabled = true;
        }
    }

    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // After time is up, switch cameras
        if (timer >= cameraActivationDuration)
        {
            // Switch camera
            CameraSwitch();
        }
    }

    void CameraSwitch()
    {
        animController.SetTrigger("fadeIn");

        // Disable the current active camera
        currentActiveCamera.enabled = false;
        currentActiveCamera.transform.GetComponent<AudioListener>().enabled = false;

        // Change the current active camera to the next camera in the array
        currentActiveCamera = cameras[++index % cameras.Length];

        // Activate the current camera
        currentActiveCamera.enabled = true;
        currentActiveCamera.transform.GetComponent<AudioListener>().enabled = true;

        // Reset the timer
        timer = 0f;
    }
}
