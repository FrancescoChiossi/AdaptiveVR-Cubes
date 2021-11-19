using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class AdaptiveEDA: MonoBehaviour
{
    public RecordBaseline recordBaseline = null;
    public LSLInput lSLInput;
    public DataLogger logger;

    public PedestrianSpawner pedestrianSpawner;

    public bool isActive = false;

    public double adaptionRate = 20.0;
    public int adaptationUp;
    public int adaptationDown;

    public double timeWindowInSeconds = 40.0;
    public double butterworthLowPassFrequency;
    public double butterworthHighPassFrequency;

    public double adaptiveFactor = 0.0;
    private double averageLast = 0.0;
    public double proportional = 0.5;

    private float nextActionTime = 0.0f;

    public double fPS;
    public int totalCount;
    public int countPerWindow;
    public double average;
    ButterworthFilter butterworthFilterLow;
    ButterworthFilter butterworthFilterHigh;

    
    [ReadOnly] public double slopeEDA = double.NaN;
    [ReadOnly] public double slopeBaseline = 1.0;
    public float slopeThreshold = 1.0f;
    // Pedestrian Spawning Variables

    public int minCount = 10;
    
    public int maxCount = 40;

    [ReadOnly] public int currencount = 0;



    public int currentCount
    {
        set { currencount = Math.Max(Math.Min(value, maxCount), minCount); }

        get { return currencount; }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == false)
            return;

        double time = UnixTime.GetTime();

        if (Time.time > nextActionTime)
        {
            nextActionTime += (float)adaptionRate;
            List<SignalSample1D> lstInput = lSLInput.samples;
            if (lstInput.Count > 0)
            {
                List<SignalSample> lst = SignalSample.convertToEDA(lstInput);
                totalCount = lst.Count;

                EDAStatistics result = GetArousalStatistics(lst, time);

                if (result != null)
                {

                    average = result.MovingAverage;

                    /*
                    double adaptiveError = -average + ultimovalor;
                    adaptiveFactor += adaptiveError * proportional;
                    Debug.Log(Math.Round(adaptiveFactor,1));
                    ultimovalor = average;
                    */


                    slopeEDA = (average - averageLast) / timeWindowInSeconds * 60.0;
                    averageLast = average;

                    slopeBaseline = recordBaseline.getBaselineSlope();
                    if (slopeEDA > slopeBaseline + slopeThreshold)
                    {
                        currentCount = pedestrianSpawner.pedestriansToSpawn;
                        currentCount -= adaptationDown;
                        pedestrianSpawner.pedestriansToSpawn = currentCount;
                        logger.writeAdaption(time, "less", currentCount, slopeBaseline, slopeEDA);
                        Debug.Log("Less LIAMS");


                    }
                    else if (slopeEDA < slopeBaseline - slopeThreshold)
                    {
                        currentCount = pedestrianSpawner.pedestriansToSpawn;
                        currentCount += adaptationUp;
                        pedestrianSpawner.pedestriansToSpawn = currentCount;

                        logger.writeAdaption(time, "more", currentCount, slopeBaseline, slopeEDA); //
                        Debug.Log("More LIAMS");
                    }
                    Debug.Log(slopeEDA + " " + slopeBaseline + " " + (slopeEDA - slopeBaseline) + " " + slopeThreshold);
                }
            }
            else
            {
                Debug.LogWarning("No EDA data!");
            }
        }
    }

   

    public EDAStatistics GetArousalStatistics(List<SignalSample> samples, double time)
    {
        if (this.timeWindowInSeconds.CompareTo(0) <= 0 || samples.Count <= 0) {
            Debug.Log("WARNING: No Samples");
            return null;
        }
        
        var samplesAffected = getAffectedSamples(samples, time); // Number of signal values that should participate in the calculation
        countPerWindow = samplesAffected.Count;
        if (countPerWindow < 3)
        {
            Debug.Log("WARNING: Less than 3 samples after filtering");
            return null;
        }

        var samplesFiltered = SignalSample.GetMedianFilterPoints(samplesAffected);

        if (butterworthFilterLow == null)
        {
            butterworthFilterLow = new ButterworthFilter(butterworthLowPassFrequency, (int)lSLInput.InfoNominalSrate, ButterworthFilter.ButterworthPassType.Lowpass);
        }
        if (butterworthFilterHigh == null)
        {
            butterworthFilterHigh = new ButterworthFilter(butterworthHighPassFrequency, (int)lSLInput.InfoNominalSrate, ButterworthFilter.ButterworthPassType.Highpass);
        }
        float[] low = butterworthFilterLow.filter(samplesFiltered);
        int i = 0;
        foreach (float v in low) {
            samplesFiltered[i].LowPassValue = v;   
            i++;
        }

        float[] high = butterworthFilterHigh.filter(samplesFiltered);
        i = 0;
        foreach (float v in high)
        {
            samplesFiltered[i].HighPassValue = v;
            i++;
        }

        InflectionLine inflectionLinesHandler = new InflectionLine();
        List<InflectionPoint> inflectionPoints = inflectionLinesHandler.GetInflectionPoints(samplesFiltered, "lowPass"); //here inflection point is calculated on samples filtered after a low pass BW

        if (inflectionPoints.Count > 0) { 
            EDATonicStatistics t = new EDATonicStatistics(samplesFiltered);
            EDAStatistics result = new EDAStatistics(samplesFiltered.ElementAt(samplesFiltered.Count -1).Time, t);
          
            //result = GetArousalInfoForInflectionPoints(inflectionPoints, timeWindow);
            //result.SCRArousalArea = GetArousalArea(highPassCoordinatesByTimeWindow, timeWindow);

            result.MovingAverage = SignalSample.GetMovingAverage(samplesFiltered);

            //result.GeneralArousalLevel = GetGeneralArousalLevel(result.MovingAverage);
            //result.SCRAchievedArousalLevel = GetPhasicLevel(result.SCRArousalArea);
            //result.LastValue = samples.ElementAt(samples.Count - 1).HighPassValue;
            //result.LastRawSignalValue = signalValues.ElementAt(signalValues.Length - 1).SignalValue;
        
            return result;
        } else
        {
            return null;
        }
    }


    public List<SignalSample> getAffectedSamples(List<SignalSample> samples, double time)
    {
        double minAcceptableTime = time - this.timeWindowInSeconds;
    
        var ret =  samples.Where(o => o.Time > minAcceptableTime).ToList();
        this.fPS = ret.Count / this.timeWindowInSeconds;
        return ret;
    }
}

