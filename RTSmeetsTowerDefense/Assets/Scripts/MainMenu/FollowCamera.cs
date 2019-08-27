using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    public Transform trajectory;
    public Transform lookAtTarget;

    Camera cam;
    Vector3 startPosition;
    Vector3 destination;
    float pathLength;
    int index = 1;
    int pointCount = 0;
    float speed = 2f;
    float startTime;

    void Start()
    {
        cam = GetComponent<Camera>();

        pointCount = trajectory.childCount;

        transform.position = trajectory.GetChild(0).transform.position;
        transform.LookAt(lookAtTarget);

        SetDestination(index);
    }

    void Update()
    {
        FollowPath();
    }

    void FollowPath()
    {
        if (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            float distanceTraveled = (Time.time - startTime) * speed;

            float pathFraction = distanceTraveled / pathLength;

            transform.position = Vector3.Lerp(startPosition, destination, pathFraction);

            transform.LookAt(lookAtTarget);
        }
        else
        {
            index = ++index % pointCount;
            SetDestination(index);
        }
        
    }

    void SetDestination(int index)
    {
        startTime = Time.time;

        destination = trajectory.GetChild(index).transform.position;

        startPosition = transform.position;

        pathLength = Vector3.Distance(transform.position, destination);
    }

    public void ActivateCamera()
    {
        cam.enabled = true;
    }
}
