using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecordBaseline : MonoBehaviour
{
    public DataLogger logger;
    public LSLInput input = null;
    
    public int countForBaselineRecording;

    public double duration = 60.0 * 3;
    public double currentDuration = 0.0;
    private double timeStart = 0.0;
    private double timeEnd = 0.0;

    public double baselineAverage = 20.0;

    public double slope = double.NaN;

    private List<float> valuesStart = new List<float>();
    private List<float> valuesEnd = new List<float>();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStart == 0.0) {
            return;
        }

        if (timeEnd != 0.0) {
            return;
        }

        if (!double.IsNaN(slope)) {
            return;
        }

        double time = UnixTime.GetTime();
        currentDuration = time - timeStart;
        if (baselineAverage > currentDuration) {
            valuesStart.Add((input.lastValues[0] / 1000) / 25);
        }

        if (duration > currentDuration && duration - baselineAverage < currentDuration)
        {
            valuesEnd.Add((input.lastValues[0] / 1000) / 25);
        }
        if (duration < currentDuration)
        {
            timeEnd = time;
            float avgStart = valuesStart.Average();
            float avgEnd = valuesEnd.Average();
            slope = (avgStart - avgEnd) / duration * 60.0;
        }
    }

    public void startRecoding() {
        timeStart = UnixTime.GetTime();
        valuesStart.Clear();
        valuesEnd.Clear();
    }

    public bool isBaselineRecoringDone()
    {
        if (timeEnd != 0) {
            return true;
        }
        else
        {
            return false;
        }

    }

    public double getBaselineSlope() 
    {
        return slope;
    }
}
