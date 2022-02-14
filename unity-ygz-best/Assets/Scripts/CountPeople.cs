using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPeople : MonoBehaviour
{
    private bool isEnabled = false;
    public int count = 0;
    public DataLogger logger;

    private double startTime = 0;

    public double timeSpan = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "visitor")
        {
            GameObject cube = col.gameObject;
           

            var timestamp = UnixTime.GetTime();
            timeSpan = timestamp - startTime;
          
            //string shirtcolor = cube.GetComponent<ShirtController>().shirtcolor;
            
            logger.writeFlow(timestamp, cube.name);
        }


        if (col.gameObject.tag == "visitor" && isEnabled) {
            count++;
        }
    }

    public int getCount()
    {
        return count;
    }

    public double getCounterTime()
    {
        return startTime;
    }

    public void setCounterEnabled(bool b)
    {
        if (b == true)
        {
            count = 0;
            startTime = UnixTime.GetTime();
        }
        isEnabled = b;
    }
}
