using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointReset : MonoBehaviour
{
    public ShirtController shirtController;
    public RandomHair randomHair;
    public TicketSpawner ticketSpawner;
   public AnimatorRandomOffset animatorRandomOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    public void reset() {
        transform.name = transform.name + "x";
        shirtController.reset();
        randomHair.reset();
        ticketSpawner.reset();
         animatorRandomOffset.reset();
    }
}
