using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

    public GameObject player;
    public float smoothness = 0.2f;
    public float smoothnessRotation = 0.2f;
    private Vector3 offset;
    public bool setOffset = false;

    void Start()
    {
        //  offset = transform.position - player.transform.position;
        offset = new Vector3(0,0,0);
    }

    void FixedUpdate() { 

        if(setOffset) {

    }
        Vector3 targetRotation = player.transform.forward;
        float singleStep = smoothnessRotation * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetRotation, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);


        Vector3 targetPosition = player.transform.position + offset;
        Vector3 smoothTransform = Vector3.Lerp(transform.position, targetPosition, smoothness);
        transform.position = smoothTransform;
 
    }
}