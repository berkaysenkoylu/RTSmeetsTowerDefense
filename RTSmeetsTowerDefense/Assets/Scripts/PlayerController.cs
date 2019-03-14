using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask layerMask;

    Camera mainCam;
    PlayerMotor playerMotor;

    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();

        mainCam = Camera.main;
        if(mainCam == null)
        {
            Debug.Log("Label your main camera correctly, or add a camera if there is a lack thereof!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, layerMask))
            {
                playerMotor.GoToLocation(hit.point);
            }
        }
    }
}
