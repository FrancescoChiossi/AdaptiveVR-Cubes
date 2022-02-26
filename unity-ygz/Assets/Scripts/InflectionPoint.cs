using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflectionPoint
{
    public double x;
    public double y;
    private int index;
    private InflectionExtremaType extremumType;

    public InflectionPoint(double x1, double y1, int indexOrigin)
    {
        this.x = x1;
        this.y = y1;
        this.index = indexOrigin;
    }

    public InflectionPoint(SignalSample signalValue, int indexOrigin, InflectionExtremaType extremum)
    {
        this.x = signalValue.Time;
        this.y = signalValue.values;
        this.index = indexOrigin;
        this.extremumType = extremum;
    }


    public InflectionExtremaType ExtremaType
    {
        get
        {
            return extremumType;
        }
        set
        {
            extremumType = value;
        }
    }

    public int IndexOrigin
    {
        get
        {
            return index;
        }
        set
        {
            x = value;
        }
    }

    public double CoordinateX
    {
        get
        {
            return x;
        }
        set
        {
            x = value;
        }
    }

    public double CoordinateY
    {
        get
        {
            return y;
        }
        set
        {
            y = value;
        }
    }
}
