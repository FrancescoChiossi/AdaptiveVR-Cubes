using System;
using System.Collections.Generic;
using UnityEngine;

public class InflectionLine
{
    private double x1;
    private double x2;
    private double y1;
    private double y2;
    private double length;
    public InflectionLineDirection direction;

    public InflectionLine()
    {
        //super();
    }

    public InflectionLine(double x1, double y1, double x2, double y2)
    {
        this.x1 = x1;
        this.x2 = x2;
        this.y1 = y1;
        this.y2 = y2;
        this.length = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        this.direction = y2 > y1 ? InflectionLineDirection.Positive : InflectionLineDirection.Negative;
    }

    public double InflectionPointX1
    {
        get
        {
            return x1;
        }
        set
        {
            x1 = value;
        }
    }

    public double InflectionPointX2
    {
        get
        {
            return x2;
        }
        set
        {
            x2 = value;
        }
    }

    public double InflectionPointY1
    {
        get
        {
            return y1;
        }
        set
        {
            y1 = value;
        }
    }

    public double InflectionPointY2
    {
        get
        {
            return y2;
        }
        set
        {
            y2 = value;
        }
    }

    public double Length
    {
        get
        {
            return length;
        }
    }

    public InflectionLineDirection Direction
    {
        get
        {
            return direction;
        }
    }

    public double GetLineLength(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
    }

    public InflectionLineDirection GetDirection(double x1, double y1, double x2, double y2)
    {
        return y2.CompareTo(y1) > 0 ? InflectionLineDirection.Positive : (y1.CompareTo(y2) == 0 ? InflectionLineDirection.Neutral : InflectionLineDirection.Negative);
    }

    /// <summary>
    /// Looking for inflection points in a list of signal values.
    /// If we have a sequence of more than two signal values with the same value (points are collinear)
    /// we take for inflection points only the first and last.
    /// </summary>
    ///
    /// <param name="signalCoordinatePoints"> List of signal values. </param>
    ///
    /// <returns>
    /// List of founded inflection points.
    /// </returns>
    public List<InflectionPoint> GetInflectionPoints(List<SignalSample> signalCoordinatePoints, string dataType)
    {
        //Debug.Log(signalCoordinatePoints.Count);

        if (signalCoordinatePoints.Count < 2) {
            return new List<InflectionPoint>();
        }

        List<InflectionPoint> inflectionPoints = new List<InflectionPoint>();
        int candidateInflectionPoint = -1;

        //add the first and the last signal value
        double firstInflectionPoints = (("default").Equals(dataType)) ? signalCoordinatePoints[0].SignalValue : (("highPass").Equals(dataType)) ? signalCoordinatePoints[0].HighPassValue : signalCoordinatePoints[0].LowPassValue;
        double secondInflectionPoints = (("default").Equals(dataType)) ? signalCoordinatePoints[1].SignalValue : (("highPass").Equals(dataType)) ? signalCoordinatePoints[1].HighPassValue : signalCoordinatePoints[1].LowPassValue;
        inflectionPoints.Add(new InflectionPoint(signalCoordinatePoints[0], 0, GetExtremaType(null, firstInflectionPoints, secondInflectionPoints)));
        double penultimateInflectionPoint = (("default").Equals(dataType)) ? signalCoordinatePoints[signalCoordinatePoints.Count - 2].SignalValue : (("highPass").Equals(dataType)) ? signalCoordinatePoints[signalCoordinatePoints.Count - 2].HighPassValue : signalCoordinatePoints[signalCoordinatePoints.Count - 2].LowPassValue;
        double lastInflectionPoint = (("default").Equals(dataType)) ? signalCoordinatePoints[signalCoordinatePoints.Count - 1].SignalValue : (("highPass").Equals(dataType)) ? signalCoordinatePoints[signalCoordinatePoints.Count - 1].HighPassValue : signalCoordinatePoints[signalCoordinatePoints.Count - 1].LowPassValue;
        inflectionPoints.Add(new InflectionPoint(signalCoordinatePoints[signalCoordinatePoints.Count - 1], signalCoordinatePoints.Count - 1, GetExtremaType(penultimateInflectionPoint, lastInflectionPoint, null)));

        double currentFindLastInflectionPoint = firstInflectionPoints;

        for (int i = 1; i < (signalCoordinatePoints.Count - 1); i++)
        {
            double currentPointY = (("default").Equals(dataType)) ? signalCoordinatePoints[i].SignalValue : (("highPass").Equals(dataType)) ? signalCoordinatePoints[i].HighPassValue : signalCoordinatePoints[i].LowPassValue;
            double previousPointY = (("default").Equals(dataType)) ? signalCoordinatePoints[i - 1].SignalValue : (("highPass").Equals(dataType)) ? signalCoordinatePoints[i - 1].HighPassValue : signalCoordinatePoints[i - 1].LowPassValue;
            double nextPointY = (("default").Equals(dataType)) ? signalCoordinatePoints[i + 1].SignalValue : (("highPass").Equals(dataType)) ? signalCoordinatePoints[i + 1].HighPassValue : signalCoordinatePoints[i + 1].LowPassValue;
            InflectionExtremaType extremumType = GetExtremaType(previousPointY, currentPointY, nextPointY);

            if (!extremumType.Equals(InflectionExtremaType.None))
            {
                if (currentPointY.CompareTo(currentFindLastInflectionPoint) != 0)
                {
                    if (i > 0 && inflectionPoints.Count > 0)
                    {
                        if ((candidateInflectionPoint + 1) == i &&
                            inflectionPoints[inflectionPoints.Count - 1].CoordinateY.Equals(currentFindLastInflectionPoint))
                        {
                            inflectionPoints.Add(new InflectionPoint(signalCoordinatePoints[candidateInflectionPoint], candidateInflectionPoint, extremumType));
                        }
                    }

                    inflectionPoints.Add(new InflectionPoint(signalCoordinatePoints[i], i, extremumType));
                }
                else
                {
                    candidateInflectionPoint = i;
                }

                currentFindLastInflectionPoint = currentPointY;
            }
            /*else
            {
                if (i > 0 && inflectionPoints.Count > 0)
                {
                    if ((candidateInflectionPoint + 1) == i && inflectionPoints[inflectionPoints.Count - 1].CoordinateY.Equals(signalCoordinatePoints[i - 1].SignalValue))
                    {
                        inflectionPoints.Add(new InflectionPoint(signalCoordinatePoints[candidateInflectionPoint], candidateInflectionPoint, extremumType));
                    }
                }
            }*/
        }

        return inflectionPoints;
    }

    /// <summary>
    /// Define the extremum type of an signal value - whether it is a minimum or maximum.
    /// </summary>
    ///
    /// <param name="signalValueList"> List with all signal value. </param>
    /// <param name="i"> Index of the target inflection point from the list with inflection points. </param>
    ///
    /// <returns>
    /// The type extremum of the target inflection point.
    /// </returns>
    private static InflectionExtremaType GetExtremaType(double? previousSignalValue, double signalValue, double? nextSignalValue)
    {
        if (!previousSignalValue.HasValue)
        {
            return (signalValue.CompareTo(nextSignalValue) >= 0) ? InflectionExtremaType.Maximum : InflectionExtremaType.Minimum;
        }
        else if (!nextSignalValue.HasValue)
        {
            return (signalValue.CompareTo(previousSignalValue) >= 0) ? InflectionExtremaType.Maximum : InflectionExtremaType.Minimum;
        }
        else if (signalValue.CompareTo(previousSignalValue) >= 0 && signalValue.CompareTo(nextSignalValue) >= 0)
        {
            return InflectionExtremaType.Maximum;
        }
        else if (signalValue.CompareTo(previousSignalValue) <= 0 && signalValue.CompareTo(nextSignalValue) <= 0)
        {
            return InflectionExtremaType.Minimum;
        }

        return InflectionExtremaType.None;
    }
}
