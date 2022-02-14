using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Text;

public class DataLogger : MonoBehaviour
{
    public int participantId = 0;

    public string rootFolder = "./LogData/";

    StreamWriter swVisitor;
    StreamWriter swSphere;
    StreamWriter swFeedback;
    StreamWriter swState;
    StreamWriter swFlow;
    StreamWriter swVisitorCount;
    StreamWriter swAdaption;

    StreamWriter swEda, swEeg, swEcg;

    StringBuilder stringbuilderEda = new StringBuilder();
    StringBuilder stringbuilderEeg = new StringBuilder();
    StringBuilder stringbuilderEcg = new StringBuilder();

    private int countedEda = 0, countedEeg = 0, countedEcg = 0;



    // Start is called before the first frame update
    public void Start()
    {
        
        if (!Directory.Exists(rootFolder))
        {
            Directory.CreateDirectory(rootFolder);
        }

        string filepath;
        filepath = rootFolder + "ID" + participantId + "-visitor.csv";

        if (File.Exists(filepath)) {
            Debug.LogError("Participant log files already exists ID " + participantId);
#if UNITY_EDITOR
            //UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif

        }

        init();

    }

    private void init() {
        // this should happen only once during the runtime
        Debug.Log("Init Files");
        string filepath = rootFolder + "ID" + participantId + "-visitor.csv";


        if (swVisitor == null)
        {
            swVisitor = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swVisitor.WriteLine("Time,Name,Type");
            swVisitor.Flush();
        }

        if (swSphere == null)
        {
            filepath = rootFolder + "ID" + participantId + "-sphere.csv";
            swSphere = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swSphere.WriteLine("Time,Type,Feedback");
            swSphere.Flush();
        }

        if (swFeedback == null)
        {
            filepath = rootFolder + "ID" + participantId + "-feedback.csv";
            swFeedback = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swFeedback.WriteLine("Time,NbackColor,CurrentColors,Trash,IsCorrect");
            swFeedback.Flush();
        }

        if (swState == null)
        {
            filepath = rootFolder + "ID" + participantId + "-state.csv";
            swState = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swState.WriteLine("Time,State,BlockNumber,AdaptationStatus");
            swState.Flush();
        }

        if (swFlow == null)
        {
            filepath = rootFolder + "ID" + participantId + "-flow.csv";
            swFlow = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swFlow.WriteLine("Time,Name");
            swFlow.Flush();
        }

        if (swEda == null)
        {
            filepath = rootFolder + "ID" + participantId + "-EDA.csv";
            swEda = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swEda.WriteLine("Time,TimeLsl,Value");
            swEda.Flush();
        }

        if (swEeg == null)
        {
            filepath = rootFolder + "ID" + participantId + "-EEG.csv";
            //swEeg = File.CreateText(filepath);
            swEeg = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swEeg.WriteLine("Time,TimeLsl,Value0,Value1,Value2,Value3,Value4,Value5,Value6,Value7");
            swEeg.Flush();
        }

        if (swEcg == null)
        {
            filepath = rootFolder + "ID" + participantId + "-ECG.csv";
            swEcg = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swEcg.WriteLine("Time,TimeLsl,Value");
            swEcg.Flush();
        }


        if (swVisitorCount == null)
        {
            filepath = rootFolder + "ID" + participantId + "-visitorCount.csv";
            swVisitorCount = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swVisitorCount.WriteLine("Time,Count,CountActual");
            swVisitorCount.Flush();
        }


        if (swAdaption == null)
        {
            filepath = rootFolder + "ID" + participantId + "-adaptation.csv";
            swAdaption = (!File.Exists(filepath)) ? File.CreateText(filepath) : File.AppendText(filepath);
            swAdaption.WriteLine("Time,Direction,CurrentCount,SlopeBaseline,SlopeEDA");
            swAdaption.Flush();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void writeVisitorClick(double timestamp, string name, bool hasTicket, string type)
    {
        if (swVisitor == null) {
            init();
        }
        swVisitor.WriteLine(timestamp + "," + name + "," + type);
        swVisitor.Flush();
    }

    public void writeSphereClick(double timestamp, string type)
    {
        if (swSphere == null)
        {
            init();
        }
        swSphere.WriteLine(timestamp + "," + type);
        swSphere.Flush();
    }

    public void writeScore(double timestamp, int nbackColor, int currentColor, string trash, bool isCorrect)
    {
        if (swFeedback == null)
        {
            init();
        }
        swFeedback.WriteLine(timestamp + "," + nbackColor + "," + currentColor + "," + trash + "," + isCorrect);
        swFeedback.Flush();
    }


    public void writeState(double timestamp, string state, int blockNumber, int adaptationStatus)
    {
        if (swState == null)
        {
            init();
        }
        swState.WriteLine(timestamp + "," + state + "," + blockNumber + "," + adaptationStatus);
        swState.Flush();
    }

    public void writeState(double timestamp, string state, int blockNumber, double adaptationStatus)
    {
        if (swState == null)
        {
            init();
        }
        swState.WriteLine(timestamp + "," + state + "," + blockNumber + "," + adaptationStatus);
        swState.Flush();
    }


    public void writeFlow(double timestamp, string name)
    {
        if (swFlow == null)
        {
            init();
        }
        swFlow.WriteLine(timestamp + "," + name );
        swFlow.Flush();
    }

    public void writeVisitorCount(double timestamp, int count, int countActual)
    {
        if (swVisitorCount == null)
        {
            init();
        }
        swVisitorCount.WriteLine(timestamp + "," + count + "," + countActual);
        swVisitorCount.Flush();
    }


    internal void writeAdaption(double timestamp, string direction, int currentCount, double slopeBaseline, double slopeEDA) //(double timestamp, string direction, int currentCount, float slopeBaseline, double slopeEDA)
    {

        if (swAdaption == null)
        {
            init();
        }
        swAdaption.WriteLine(timestamp  + "," + direction + "," + currentCount + "," + slopeBaseline + "," + slopeEDA); //(timestamp + "," + direction + "," + currentCount + "," + slopeBaseline + "," + slopeEDA);
        swAdaption.Flush();
    }


    internal void write(string name, SignalSample1D s)
    {
        if (swEda == null || swEeg == null || swEcg == null)
        {
            init();
        }
        if (name.ToLower() == "eda")
        {
            stringbuilderEda.AppendFormat("{0},{1},{2}{3}", s.time, s.timeLsl, s.values[0], Environment.NewLine);

            countedEda++;
            if (countedEda % 1000 == 0)
            {
                swEda.WriteLine(stringbuilderEda);
                stringbuilderEda.Clear();
                swEda.Flush();
            }
        }
        else if (name.ToLower() == "eeg")
        {
            if (s.values.Length == 8)
            {
                stringbuilderEeg.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}{10}", s.time, s.timeLsl, s.values[0], s.values[1], s.values[2], s.values[3], s.values[4], s.values[5], s.values[6], s.values[7], Environment.NewLine);
            }
            else {
                throw new NotImplementedException("Your electrode count is not 8 please ajust the script");
            }

            countedEeg++;
            if (countedEeg % 1000 == 0)
            {
                swEeg.WriteLine(stringbuilderEeg);
                stringbuilderEeg.Clear();
                swEeg.Flush();
            }
        }
        else if (name.ToLower() == "ecg")
        {
            if (s.values.Length == 1)
            {
                stringbuilderEcg.AppendFormat("{0},{1},{2}{3}", s.time, s.timeLsl, s.values[0], Environment.NewLine);
            }
            else
            {
                throw new NotImplementedException("Your electrode count is not 1 please ajust the script");
            }
            countedEcg++;
            if (countedEcg % 1000 == 0)
            {
                swEcg.WriteLine(stringbuilderEcg);
                stringbuilderEcg.Clear();
                swEcg.Flush();
            }
        }
        else
        {
            Debug.LogWarning("Logger Data Dropped");
        }
    }

    void OnDestroy()
    {
        if (swEda != null)
        {
            swEda.Flush();
        }
        if (swEeg != null)
        {
            swEeg.Flush();
        }
        if (swEcg != null)
        {
            swEcg.Flush();
        }
    }
}
