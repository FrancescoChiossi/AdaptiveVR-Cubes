using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SignalSample: ICloneable
{

    public double time;
    public double timeLsl;
    public float values;
    private float highPassValue;
    private float lowPassValue;

    public SignalSample()
    {
    }


    public SignalSample(SignalSample signal)
    {
        this.time = signal.Time;
        this.timeLsl = signal.timeLsl;
        this.values = signal.values;
    }

    public SignalSample(SignalSample1D signal)
    {
        this.time = signal.Time;
        this.timeLsl = signal.timeLsl;
        this.values = signal.values[0];
        if (signal.values.Length > 1) {
            Debug.LogWarning("WARNING: data is beeing droped");
        }
    }

    public SignalSample(double lastTimeStamp, double timeLsl, float samples)
    {
        this.time = lastTimeStamp;
        this.timeLsl = timeLsl;
        this.values = samples;
    }


    public SignalSample(double time, float samples, float highPassValue, float lowPassValue)
    {
        this.time = time;
        this.values = samples;
        this.highPassValue = highPassValue;
        this.lowPassValue = lowPassValue;
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

    public float Values
    {
        get
        {
            return this.values;
        }

        set
        {
            this.values = value;
        }
    }
    public float SignalValue
    {
        get
        {
            return this.values;
        }
    }



    public float HighPassValue
    {
        get
        {
            return highPassValue;
        }

        set
        {
            highPassValue = value;
        }
    }

    public float LowPassValue
    {
        get
        {
            return lowPassValue;
        }

        set
        {
            lowPassValue = value;
        }
    }
    #endregion Properties


    public object Clone()
    {
        return new SignalSample(this.time, this.timeLsl, this.values);
    }

    public static List<SignalSample> convertToEDA(List<SignalSample1D> lst)
    {
        List<SignalSample> ret = new List<SignalSample>();
        foreach (SignalSample1D s in lst)
        {
            SignalSample s1 = new SignalSample(s);
            s1.values = (s1.values / 1000) / 25;
            ret.Add(s1);

        }
        return ret;
    }

    public static List<SignalSample> convert (List<SignalSample1D> lst)
    {
        List<SignalSample> ret = new List<SignalSample>();
        foreach (SignalSample1D s in lst)
        {
            ret.Add(new SignalSample(s));

        }
        return ret;
    }
    
    public static double GetMovingAverage(List<SignalSample> samples)
    {
        double movingAverage = 0;
        foreach (var s in samples)
        {
            movingAverage += s.values;
        }

        return movingAverage / samples.Count;
    }
    
    public static List<SignalSample> GetMedianFilterPoints(List<SignalSample> lst)
    {
        if (lst == null) return null;

        //TODO: current verion runs out of array.
        //SignalDataByTime[] signalCoordinatesExtented = extendSignalCoordinates(signalCoordinates);

        List<SignalSample> result = new List<SignalSample>();

        for (int i = 2; i < lst.Count - 2; ++i)
        {
            float[] window = new float[5];
            //store each 5 consecutive elements
            for (int j = 0; j < 5; ++j)
            {
                window[j] = lst.ElementAt(i - 2 + j).SignalValue;
            }

            //sort elements of the array window
            for (int j = 0; j < 3; ++j)
            {
                //   Find position of minimum element
                int min = j;
                for (int k = j + 1; k < 5; ++k)
                    if (window[k] < window[min])
                        min = k;
                //   Put found minimum element in its place
                float temp = window[j];
                window[j] = window[min];
                window[min] = temp;
            }

            //   Get result - the middle element
            SignalSample s = new SignalSample();
            //signalCoordinates.ElementAt(i - 2).Time, window[2], signalCoordinates.ElementAt(i - 2).HighPassValue, signalCoordinates.ElementAt(i - 2).LowPassValue
            s.values = window[2];
            result.Add(s);
        }

        return result;
    }

    public override string ToString()
    {
        return values.ToString() + "_"; // '_' is used for splitting in server
    }


}
