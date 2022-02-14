using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Cubecontroller : MonoBehaviour
{
    [ReadOnly] public string destination;
    [ReadOnly] public Vector3 destinationVec;
    Vector3 lastPosition;
    public bool reachedDestination;
    public float minDistance = 1.5f;
    public float rotationSpeed;
    public float minSpeed, maxSpeed;
    public float movementSpeed;
    Vector3 velocity;

    [ReadOnly] public float destinationDistance = 0.0f;

    private void Start()
    {
        movementSpeed = Random.Range(minSpeed, maxSpeed);
    }
    private void Update()
    {
        if (transform.position != destinationVec)
        {
            Vector3 destinationDirection = destinationVec - transform.position;
            destinationDirection.y = 0;

            destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= minDistance)
            {
                reachedDestination = false;
                transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
                transform.Translate(destinationDirection * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;
            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            var rightDotProduct = Vector3.Dot(transform.right, velocity);
        }
    }

    public void SetDestination(Waypoint destination)
    {
        this.destination = destination.getName();
        this.destinationVec = destination.GetPosition();
        reachedDestination = false;
    }
}