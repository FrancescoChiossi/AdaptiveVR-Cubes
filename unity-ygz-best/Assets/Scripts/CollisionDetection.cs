using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public string trashColor = "";
    public Mytask mytask;
   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter (Collision col)
    {
        double timestamp = UnixTime.GetTime();
        if (col.gameObject.name == "NbackBall" && col.gameObject.GetComponent<CollisionDone>().isCollision == false)
        {
            col.gameObject.GetComponent<CollisionDone>().isCollision = true;
            mytask.collision(timestamp, trashColor);
            Destroy(col.gameObject);
        }
    }
}

