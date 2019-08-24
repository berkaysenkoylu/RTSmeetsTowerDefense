using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask layerMask; // Layer mask for the ground (walkable navmesh)

    Camera mainCam; // Reference for main camera (used for ray casting)
    PlayerMotor playerMotor; // Reference for player motor script

    void Start()
    {
        // Set the reference for player motor script
        playerMotor = GetComponent<PlayerMotor>();

        // Set reference for main camera in the scene
        mainCam = Camera.main;

        // If maincam reference is null, notify the dev
        if(mainCam == null)
        {
            Debug.Log("Label your main camera correctly, or add a camera if there is a lack thereof!");
        } 
    }

    void Update()
    {
        // Check if mouse is over a UI element; if so, do nothing
        // TODO: Perhaps change it later?
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
        {
            return;
        }

        // Check for mouse left clicks
        if (Input.GetMouseButtonDown(0))
        {
            // Form a ray which goes from main camera through a screen point(mouse position in this case)
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            // Form a raycasthit variable to store the hit information
            RaycastHit hit;

            // Handle the raycast: using the formed ray, onto the specified layermask ("Ground" in this case)
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                // If player is collecting a resource, do nothing
                if (playerMotor.IsPlayerCollecting())
                    return;
                
                // If the ray hits on an object whose layer is 'Resource'
                if (hit.collider.gameObject.layer == 10)
                {
                    hit.collider.gameObject.GetComponent<Resource>().HighlightResource();

                    // Check the distance; if close enough, start resource collection (see PlayerMotor.cs' Update() method) 
                    if (Vector3.Distance(transform.position, hit.point) <= 3.0f) // TODO: Tweak this number
                    {
                        Debug.Log("Close enough");

                        // Set the target resource to the hit object
                        playerMotor.SetTargetResource(hit.collider.gameObject);
                    }
                    else
                    {
                        Debug.Log("Not close enough");
                        // If not close enough, we need to get sufficiently close to the resource first
                        
                        // Set the target resource
                        playerMotor.SetTargetResource(hit.collider.gameObject);

                        // Go to nearby location. When reached the location, resource collection will start (see PlayerMotor.cs' Update() method) 
                        playerMotor.GoToLocation(hit.point);
                    }
                }
                else
                {
                    // If the ray hits on terrain or any other obstacle

                    // Check the hit distance: if too close to the player, do nothing
                    if (Vector3.Distance(transform.position, hit.point) <= 1.0f)
                        return;

                    // Reset the target resource
                    playerMotor.ResetTargetResource();

                    // Go to the hit location
                    playerMotor.GoToLocation(hit.point);
                }
            }
        }
    }
}
