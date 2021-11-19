using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyandPlay4s : MonoBehaviour
{
    public Mytask task;

    bool isTouched = false;
    public float timeRemaining = 4;

    public void Start()
    { 
    }

    
    void Update()
    {
        // Is timer done? And not Touched?

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        
        if (timeRemaining < 0 && isTouched ==false)
        {
            //Debug.Log("Kill");
            
            TODO: Add logging when sphere gets destroyed becasue of time out
            
            Destroy(this.gameObject);
            if (task != null)
                task.generateSpheres();
            else
                Debug.LogError("No reference");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger " + other.name);
        if (other.gameObject.name == "Cube")
        {
            //Set its touched and destroy THIS script not the object itself.
            isTouched = true;
            Destroy(this);
        }
    }
}
