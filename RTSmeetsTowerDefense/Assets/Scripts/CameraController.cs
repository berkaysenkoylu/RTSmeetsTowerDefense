using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f; // Variable for camera speed
    public float zoomSpeed = 10f; // Variable for camera zoom speed
    public float zOffset = 15f; // Variable for distance between player and camera in z axis (used if space bar is pressed)
    public bool isEdgeScrolling = false; // Variable to check if edge scrolling is being utilized
    public Vector2 xMinMax; // Variable for min and max of x axis of the camera position
    public Vector2 yMinMax; // Variable for min and max of y axis of the camera position
    public Vector2 zMinMax; // Variable for min and max of z axis of the camera position
    public Transform playerTransform; // Variable for character transform

    public bool enableEdgeScrolling = false;

    Vector2 mousePosCorrespondance; // Variable to find where the mouse position corresponds to on screen

    void Start()
    {

    }

    void Update()
    {
        // Check if the edge scrolling is being done right now
        isEdgeScrolling = isEdgeScrollingNow();

        // If not, call normal movement function
        if (!isEdgeScrolling)
        {
            NormalMovement();
        }

        // If edge scrolling is being done, then movement is done with it
        if(isEdgeScrolling && enableEdgeScrolling)
        {
            EdgeScrollMovement();
        }

        // Call camera zooming function to handle zooming
        CameraZooming();

        // If space bar is pressed, reset the camera position so that player is in the middle of the screen (roughly)
        if(playerTransform != null && Input.GetButtonDown("Jump"))
        {
            transform.position = new Vector3(playerTransform.position.x,
                                             transform.position.y,
                                             playerTransform.position.z - zOffset);
        }
    }

    // Function for camera movement
    void CameraMovement(float hor, float ver)
    {
        // Translate the camera with the utilization of hor and ver arguments
        transform.Translate(new Vector3(hor, 0f, ver) * speed * Time.deltaTime, Space.World);
    }

    // Function for movement with buttons (WASD and arrow buttons)
    void NormalMovement()
    {
        // Form horizontal and vertical float values, depending on button presses
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Call the function camera movement by using formed horizontal and vertical float values as arguments
        CameraMovement(horizontal, vertical);

        // Call the clamp camera position function to clamp the camera position so that player won't be able to go off of the map
        ClampCameraPosition();
    }

    // Function for mouse edge scrolling
    void EdgeScrollMovement()
    {
        // Form the horizontal and vertical float values to emulate Input.GetAxis("Horizontal") or Input.GetAxis("Vertical")
        float horizontal = 0f;
        float vertical = 0f;

        // Set up a vector whose components are normalized mouse position coordinates with respect to screen width and height
        mousePosCorrespondance.x = Input.mousePosition.x / Screen.width;
        mousePosCorrespondance.y = Input.mousePosition.y / Screen.height;

        // Check if the mouse is within the 5% of the left edge of the screen
        if(mousePosCorrespondance.x <= 0.05f)
        {
            // Set the horizontal to -1 (movement to left)
            horizontal = -1f;
        }

        // Check if the mouse is within the 5% of the right edge of the screen
        if (mousePosCorrespondance.x >= 0.95f)
        {
            // Set the horizontal to -1 (movement to right)
            horizontal = 1f;
        }

        // Check if the mouse is within the 5% of the bottom edge of the screen
        if (mousePosCorrespondance.y <= 0.05f)
        {
            // Set the vertical to -1 (movement to bottom)
            vertical = -1f;
        }

        // Check if the mouse is within the 5% of the top edge of the screen
        if (mousePosCorrespondance.y >= 0.95f)
        {
            // Set the vertical to -1 (movement to top)
            vertical = 1f;
        }

        // Call the camera movement function using the formed horizontal and vertical floats values as arguments
        CameraMovement(horizontal, vertical);
    }

    // Function for checking if player is side scrolling or not
    bool isEdgeScrollingNow()
    {
        // Set up a vector whose components are normalized mouse position coordinates with respect to screen width and height
        mousePosCorrespondance.x = Input.mousePosition.x / Screen.width;
        mousePosCorrespondance.y = Input.mousePosition.y / Screen.height;

        // Return true if the mouse is at the 5% of the edges of the screen, otherwise return false
        return ((mousePosCorrespondance.x <= 0.05f || mousePosCorrespondance.x >= 0.95f)
            || (mousePosCorrespondance.y <= 0.05f || mousePosCorrespondance.y >= 0.95f)
            && (Input.GetAxis("Horizontal") == 0f || Input.GetAxis("Vertical") == 0f));
    }

    // Function for clamping the camera with specified x, y and z coordinates
    void ClampCameraPosition()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMinMax.x, xMinMax.y),
                                         Mathf.Clamp(transform.position.y, yMinMax.x, yMinMax.y),
                                         Mathf.Clamp(transform.position.z, zMinMax.x, zMinMax.y));
    }

    // Function for handling the camera zoom
    void CameraZooming()
    {
        // Get the mouse scrollwheel axis and multiply it with zoom speed
        float zooming = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        // Check if camera height is at the min or max y axis values, if so; set zooming to 0
        if (transform.position.y <= yMinMax.x && Input.GetAxis("Mouse ScrollWheel") > 0f || 
            transform.position.y >= yMinMax.y && Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            zooming = 0f;
        }

        // Form a trajectory vector. The coordinates are with respect to world coordinates, thus we take cos and sin of the x angle of the camera
        Vector3 trajectory = new Vector3(0f, -zooming * Mathf.Sin(transform.rotation.x), zooming * Mathf.Cos(transform.rotation.x));

        // Having found the trajectory vector, just use it to translate the camera
        transform.Translate(trajectory, Space.World);
    }
}
