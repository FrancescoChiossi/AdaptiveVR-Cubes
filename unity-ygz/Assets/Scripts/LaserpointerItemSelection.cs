using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class LaserpointerItemSelection : MonoBehaviour
{

    public DataLogger logger;
    public SteamVR_LaserPointer laserPointer;
    bool[] triggable = new bool[2];
    public UnityEvent onTriggerUp;

    public GameObject activeObject;


    void Awake()
    {
        laserPointer.PointerClick += PointerClick;
    }
    
    public void PointerClick(object sender, PointerEventArgs e)
    {
        double timestamp = UnixTime.GetTime();
        if (e.clickState == ClickState.Down)
        {
            //Debug.Log("TRIGGER DOWN");
            if (e.target.tag == "visitor")
            {
                //Debug.Log("TODO: Check Ticket DOWN");
                logger.writeVisitorClick(timestamp, e.target.name, e.target.GetComponent<TicketSpawner>().hasticket, "down");
            }
            if (e.target.tag == "nbacktask")
            {
                //Debug.Log("TODO: Check nbacktask DOWN");
                logger.writeSphereClick(timestamp, "down");

                Rigidbody r = e.target.gameObject.GetComponent<Rigidbody>();
                if (r != null) { 
                    r.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                } else
                {
                    Debug.Log(e.target.gameObject.name);
                }
                activeObject = e.target.gameObject;
                activeObject.transform.parent = gameObject.transform;
            }
        }
        else if (e.clickState == ClickState.Up)
        {
            //Debug.Log("TRIGGER UP");
            if (e.target.tag == "visitor")
            {
                //Debug.Log("TODO: Check Ticket UP");
                logger.writeVisitorClick(timestamp, e.target.name, e.target.GetComponent<TicketSpawner>().hasticket, "up");
                e.target.GetComponent<ShirtController>().setSelected();
            }
            if (e.target.tag == "nbacktask")
            {
                //Debug.Log("TODO: Check nback UP");
                logger.writeSphereClick(timestamp, "up");
                if (activeObject != null)
                {
                    Rigidbody r = activeObject.GetComponent<Rigidbody>();
                    r.constraints = RigidbodyConstraints.None;
                    activeObject.transform.parent = null;
                }
            }
        }
    }

  

    //-------------------------------------------------
    private void Start()
    {
        triggable[0] = true;
        triggable[1] = true;
         
    }
    //-------------------------------------------------


    void Update()
    {
        //activeObject.transform 0 

    }

    public void manualTriggerUp()
    {
        onTriggerUp.Invoke();
    }

}
//}
