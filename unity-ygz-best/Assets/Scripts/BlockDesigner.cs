using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlockDesigner : MonoBehaviour
{
    public List<int> order = new List<int>{ 1, 2, 3, 4 };

    private double timeStart = 0.0;
    public double duration = 600.0;
    public double currentDuration = 0.0;

    public bool isDone = false;

    public int counter = -1;

    // Start is called before the first frame update
    void Start()
    {
        order.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        double timeNow = UnixTime.GetTime();
        currentDuration = timeNow - timeStart;
        if (currentDuration >= duration) {
            isDone = true;
        }
    }

    public void startRecoding()
    {
        timeStart = UnixTime.GetTime();
        isDone = false;
        counter++;
    }

    public int getCurrentBlock() {

        if (counter == -1) {
            return -1;
        }
        else if (counter >= order.Count) {
            return -2;
        }
        else
        {
            return order[counter];
        }
    }

    public int getNextBlock()
    {
        int c = counter + 1;
        if (c == -1)
        {
            return -1;
        }
        else if (c >= order.Count)
        {
            return -2;
        }
        else
        {
            return order[c];
        }
    }
}
