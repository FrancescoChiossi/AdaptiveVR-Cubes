using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class RecordBaseline : MonoBehaviour
{
    public DataLogger logger;
    public LSLInput input = null;
    
    public int countForBaselineRecording;

    public double duration = 60f * 2;
    public double currentDuration = 0.0;
    private double timeStart = 0.0;
    private double timeEnd = 0.0;

    public double baselineAverage = 20.0;

    public double tonic = double.NaN;


    private List<float> valuesStart = new List<float>();
    private List<float> valuesEnd = new List<float>();

    public tcpClient tcp;
    public Mytask mytask;

    private void Start()
    {
        
    }


    // Start is called before the first frame update
    private void Update()
    {
        if (timeStart == 0.0)
        {
            return;
        }

        if (timeEnd != 0.0)
        {
            return;
        }

        if (!double.IsNaN(tonic))
        {
            return;
        }

        // Fetch tonic data
        List<SignalSample1D> lstInput = input.samples;
        List<SignalSample> lst = SignalSample.convertToEDA(lstInput);

        string outputStr = "";
        foreach (var value in lst)
        {
            outputStr += value.ToString(); // Convert data to appropriate format for server
        }
        List<float> tonicData = tcp.SendMessage(outputStr);
        Debug.Log(tonicData.Count);

        double time = UnixTime.GetTime();
        currentDuration = time - timeStart;
        if (baselineAverage > currentDuration)
        {
            valuesStart.Add(tonicData[0] / 1000 / 25);
        }

        if (duration > currentDuration && duration - baselineAverage < currentDuration)
        {
            valuesEnd.Add((tonicData[0] / 1000) / 25);
        }
        if (duration < currentDuration)
        {
            timeEnd = time;
            float avgStart = valuesStart.Average();
            float avgEnd = valuesEnd.Average();
            tonic = (avgStart - avgEnd) / duration * 60.0;
        }
    }

    // Update is called once per frame
    /*async void Update()
    {
        if (timeStart == 0.0) {
            return;
        }

        if (timeEnd != 0.0) {
            return;
        }

        if (!double.IsNaN(tonic)) {
            return;
        }

        // Fetch tonic data
        List<SignalSample1D> lstInput = input.samples;
        List<SignalSample> lst = SignalSample.convertToEDA(lstInput);
        string outputStr = "";
        foreach (var value in lst)
        {
            outputStr += value.ToString(); // Convert data to appropriate format for server
        }
        List<float> tonicData = await tcp.SendMessage("-0.010214_0.009999_0.011373_0.011115_0.010600_-0.005107_0.006480_0.005965_0.005879_0.007081_-0.010214_0.009999_0.011373_0.011115_0.010600_-0.005107_0.006480_0.005965_0.005879_0.007081_-0.010214_0.009999_0.011373_0.011115_0.010600_-0.005107_0.006480_0.005965_0.005879_0.007081_");
        Debug.Log(tonicData.Count);

        double time = UnixTime.GetTime();
        currentDuration = time - timeStart;
        if (baselineAverage > currentDuration) {
            valuesStart.Add(tonicData[0] / 1000 / 25);
        }

        if (duration > currentDuration && duration - baselineAverage < currentDuration)
        {
            valuesEnd.Add((tonicData[0] / 1000) / 25);
        }
        if (duration < currentDuration)
        {
            timeEnd = time;
            float avgStart = valuesStart.Average();
            float avgEnd = valuesEnd.Average();
            tonic = (avgStart - avgEnd) / duration * 60.0;
        }
    }*/

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
        return tonic;
    }
}
