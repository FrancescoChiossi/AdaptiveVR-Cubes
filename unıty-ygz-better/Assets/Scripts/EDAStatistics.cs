using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDAStatistics
{

    public double time;
    public double movingAverage;

    
    private EDATonicStatistics tonicStatistics;

    public EDAStatistics()
    {
    }

    public EDAStatistics(double lastTimeStamp, EDATonicStatistics tonicStatistics)
    {
        this.time = lastTimeStamp;
        this.tonicStatistics = tonicStatistics;
    }


    #region Properties
    public double Time
    {
        get
        {
            return this.time;
        }

        set
        {
            this.time = value;
        }
    }

    public double MovingAverage
    {
        get
        {
            return this.movingAverage;
        }

        set
        {
            this.movingAverage = value;
        }
    }
    #endregion Properties
}
