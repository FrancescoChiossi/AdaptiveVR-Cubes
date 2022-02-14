using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public class EDATonicStatistics
{
    private double slope;
    private double meanAmp;
    private double minAmp;
    private double maxAmp;
    private double stdDeviation;

    public EDATonicStatistics()
    {

    }

    public EDATonicStatistics(List<SignalSample> samples)
    {

        InflectionLine inflectionLinesHandler = new InflectionLine();
        List<InflectionPoint> inflectionPoints = inflectionLinesHandler.GetInflectionPoints(samples, "lowPass");
        calcTonicStatisticsForPoints(inflectionPoints);
    }

    private void calcTonicStatisticsForPoints(List<InflectionPoint> inflectionPoints)
    {

        if (inflectionPoints.Count < 1) {
            return;
        }

        double tonicCoordinateXFirst = inflectionPoints.ElementAt(0).CoordinateX;
        double tonicCoordinateXLast = inflectionPoints.ElementAt(inflectionPoints.Count - 1).CoordinateX;
        double tonicCoordinateYFirst = inflectionPoints.ElementAt(0).CoordinateY;
        double tonicCoordinateYLast = inflectionPoints.ElementAt(inflectionPoints.Count - 1).CoordinateY;
        this.MeanAmp = (tonicCoordinateYFirst + tonicCoordinateYLast) / 2;
        this.Slope = (tonicCoordinateYLast - tonicCoordinateYFirst) / (tonicCoordinateXLast - tonicCoordinateXFirst);
        List<double> allMaximums = new List<double>();
        double minTonic = inflectionPoints.ElementAt(0).CoordinateY;
        double maxTonic = inflectionPoints.ElementAt(inflectionPoints.Count - 1).CoordinateY;

        double sumMaximums = 0;

        for (int i = 0; i < inflectionPoints.Count; i++)
        {
            if (inflectionPoints.ElementAt(i).ExtremaType.Equals(InflectionExtremaType.Maximum))
            {
                double currentY = inflectionPoints.ElementAt(i).CoordinateY;
                minTonic = (minTonic.CompareTo(currentY) > 0 && !currentY.Equals(0.0)) ? currentY : minTonic;
                maxTonic = (maxTonic.CompareTo(currentY) < 0) ? currentY : maxTonic;

                double currentAmplitude = currentY;
                allMaximums.Add(currentAmplitude);
                sumMaximums += currentAmplitude;
            }
        }

        this.MinAmp = minTonic;
        this.MaxAmp = maxTonic;
        double mean = GetTonicMean(allMaximums, sumMaximums, this.MeanAmp);

        this.StdDeviation = GetStandardDeviation(allMaximums, mean);
        
    }

    /***
     * 
     * Sven: Double check this. 
     * 
     ***/
    private static double GetTonicMean(List<double> allMaximums, double sumMaximums, double tonicMeanAmp)
    {
        if (allMaximums.Count == 1) return tonicMeanAmp;
        return (allMaximums != null && allMaximums.Count > 0.0) ? (sumMaximums / allMaximums.Count) : 0.0;
    }

    /// <summary>
    /// Calculate standard deviation of a list of numbers 
    /// </summary>
    ///
    /// <param name="listOfNumbers"> List of numbers. </param>
    /// <param name="mean"> Mean of the list listOfNumbers. </param>
    ///
    /// <returns>
    /// Standard deviation.
    /// </returns>
    private double GetStandardDeviation(List<double> listOfNumbers, double mean)
    {
        double stdDeviation = 0;
        foreach (double currentNumber in listOfNumbers)
        {
            stdDeviation += Math.Pow((currentNumber - (double)mean), 2);
        }

        return (listOfNumbers.Count > 0) ? Math.Sqrt(stdDeviation / listOfNumbers.Count) : 0;
    }

    public double Slope
    {
        get
        {
            return slope;
        }

        set
        {
            slope = value;
        }
    }

    public double MeanAmp
    {
        get
        {
            return meanAmp;
        }

        set
        {
            meanAmp = value;
        }
    }

    public double MinAmp
    {
        get
        {
            return minAmp;
        }

        set
        {
            minAmp = value;
        }
    }

    public double MaxAmp
    {
        get
        {
            return maxAmp;
        }

        set
        {
            maxAmp = value;
        }
    }

    public double StdDeviation
    {
        get
        {
            return stdDeviation;
        }

        set
        {
            stdDeviation = value;
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append("Slope: " + slope + ": \n");
        str.Append("Minimum value: " + minAmp + "\n");
        str.Append("Maximum value: " + maxAmp + "\n");
        str.Append("Mean value: " + meanAmp + "\n");
        str.Append("Standard deviation: " + stdDeviation + "\n");

        return str.ToString();
    }
    
}
