using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterworthFilter
{
    public enum ButterworthPassType
    {
        //phasic signal
        Highpass,
        //tonic signal
        Lowpass
    }

    private readonly double resonance = Math.Sqrt(2);

    private readonly double frequency;
    private readonly int sampleRate;
    private readonly ButterworthPassType passType;

    private double c, a1, a2, a3, b1, b2;


    /// <summary>
    /// Array of input values, latest are in front
    /// </summary>
    private double[] inputHistory = new double[2];

    /// <summary>
    /// Array of output values, latest are in front
    /// </summary>
    private double[] outputHistory = new double[3];


    public ButterworthFilter(double frequency, int sampleRate, ButterworthPassType passType)
    {
        this.frequency = frequency;
        this.sampleRate = sampleRate;
        this.passType = passType;
        //this.resonance = resonance;
            /*
             -  we used a 1st order, low-pass Butterworth filter set to 0.05 Hz to extract the tonic signal.
             */
    }

    public float[] filter(List<SignalSample> lst)
    {

        this.inputHistory[0] = 0.0;
        this.inputHistory[1] = 0.0;
        this.outputHistory[0] = 0.0;
        this.outputHistory[1] = 0.0;
        this.outputHistory[2] = 0.0;

        float[] ret = new float[lst.Count];
        int i = 0;
        foreach (SignalSample s in lst)
        {
            ret[i] = (float)update(s.values);
            i++;
        }
        return ret;
    }

    private double update(double newInput)
    {
        double newOutput = a1 * newInput + a2 * this.inputHistory[0] + a3 * this.inputHistory[1] - b1 * this.outputHistory[0] - b2 * this.outputHistory[1];

        this.inputHistory[1] = this.inputHistory[0];
        this.inputHistory[0] = newInput;

        this.outputHistory[2] = this.outputHistory[1];
        this.outputHistory[1] = this.outputHistory[0];
        this.outputHistory[0] = newOutput;

        return newOutput;
    }

    private void SetButterworthParameters(ButterworthPassType passType)
    {
        switch (passType)
        {
            case ButterworthPassType.Lowpass:
                c = 1.0 / Math.Tan(Math.PI * frequency / sampleRate);
                //a1 = 1.0 / (1.0 + c * c);
                a1 = 1.0 / (1.0 + resonance * c + c * c);
                a2 = 2.0 * a1;
                a3 = a1;
                b1 = 2.0 * (1.0 - c * c) * a1;
                //b2 = (1.0 + c * c) * a1;
                b2 = (1.0 - resonance * c + c * c) * a1;
                break;
            case ButterworthPassType.Highpass:
                c = Math.Tan(Math.PI * frequency / sampleRate);
                //a1 = 1.0 / (1.0 + c * c);
                a1 = 1.0 / (1.0 + resonance * c + c * c);
                a2 = -2.0 * a1;
                a3 = a1;
                b1 = 2.0 * (c * c - 1.0) * a1;
                //b2 = (1.0 + c * c) * a1;
                b2 = (1.0 - resonance * c + c * c) * a1;
                break;
        }
    }
}
