using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketSpawner : MonoBehaviour

{
    public GameObject ticketRight;
    public GameObject ticketLeft;
    public bool hasticket;

    public double chance = .9;

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

 
   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void reset()
    {
        float number = Random.Range(0.0f, 1.0f);
        bool isRight = (Random.value >= 0.5f);


        ticketRight.SetActive(false);
        ticketLeft.SetActive(false);
        hasticket = false;

        if (number > chance && isRight == true)
        {
            //Debug.Log("new Ticket Spawned");
            ticketRight.SetActive(true);
            hasticket = true;


        }
        else if (number > chance && isRight == false)
        {
            //Debug.Log("new Ticket Spawned");
            ticketLeft.SetActive(true);
            hasticket = true;
        }
    }
}
