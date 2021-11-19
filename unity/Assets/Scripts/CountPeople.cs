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
            GameObject liam = col.gameObject;
            var randomHair = liam.GetComponent<RandomHair>();

            var timestamp = UnixTime.GetTime();
            timeSpan = timestamp - startTime;
            bool hasticket = liam.GetComponent<TicketSpawner>().hasticket;
            string shirtcolor = liam.GetComponent<ShirtController>().shirtcolor;
            string hair = randomHair.liamhair;
            logger.writeFlow(timestamp, liam.name, hasticket, shirtcolor, hair);
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
