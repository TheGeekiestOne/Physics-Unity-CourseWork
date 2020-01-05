using UnityEngine;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public struct Line
{
    public readonly float M; // Slope
    public readonly float C; // Constant function offset
    public readonly float OffsetX; // Offset for x

    /// <summary>
    /// Construct a line with slope M and offset C
    /// </summary>
    public Line(float m, float c)
    {
        M = m;
        C = c;
        OffsetX = 0;
    }

    public Line(float m, float x, float y)
    {
        M = m;
        C = y;
        OffsetX = x;
    }

    /// <summary>
    /// Construct a line going through p1 and p2
    /// </summary>
    public Line(float x1, float y1, float x2, float y2)
    {
        M = (y2 - y1) / (x2 - x1);
        OffsetX = x1;
        C = y1;
    }

    public bool PointOnLineXY(Vector3 p)
    {
        return Mathf.Approximately(p.y, Evaluate(p.x));
    }

    public bool PointOnLineXZ(Vector3 p)
    {
        return Mathf.Approximately(p.z, Evaluate(p.x));
    }

    /// <summary>
    /// Evaluates Y/Z using MX + C
    /// </summary>
    public float Evaluate(float x)
    {
        return M * (x - OffsetX) + C;
    }

    public static bool IntersectXY(Line l1, Line l2, out Vector3 point)
    {
        point = Vector3.zero;
        if (Mathf.Approximately(l1.M, l2.M)) return false;
        var x = (l2.C - l1.C + (l1.M * l1.OffsetX) - (l2.M * l2.OffsetX)) / (l1.M - l2.M);
        var y = l1.M * (x - l1.OffsetX) + l1.C;
        point = new Vector3(x, y);
        return true;
    }
    
    public static bool IntersectXZ(Line l1, Line l2, out Vector3 point)
    {
        point = Vector3.zero;
        if (Mathf.Approximately(l1.M, l2.M)) return false;
        var x = (l2.C - l1.C + (l1.M * l1.OffsetX) - (l2.M * l2.OffsetX)) / (l1.M - l2.M);
        var y = l1.M * (x - l1.OffsetX) + l1.C;
        point = new Vector3(x, 0, y);
        return true;
    }
}