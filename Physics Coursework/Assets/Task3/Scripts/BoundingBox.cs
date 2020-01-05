using UnityEngine;
using static Vector3DMaths;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public struct BoundingBox
{
    public Vector3 Center, Extents;

    /// <summary>
    ///   <para>The Minimal point of the box. This is always equal to center-extents.</para>
    /// </summary>
    public Vector3 Min
    {
        get => SubtractVectors(Center, Extents);
        set => SetMinMax(value, Max);
    }

    /// <summary>
    ///   <para>The Maximal point of the box. This is always equal to center+extents.</para>
    /// </summary>
    public Vector3 Max
    {
        get => AddVectors(Center, Extents);
        set => SetMinMax(Min, value);
    }

    public Vector3 Size => ScaledVector(Extents, 2);

    public BoundingBox(Vector3 center, Vector3 size)
    {
        Center = center;
        Extents = ScaledVector(size, 0.5f);
    }

    /// <summary>
    ///   <para>Sets the bounds to the Min and Max value of the box.</para>
    /// </summary>
    public void SetMinMax(Vector3 min, Vector3 max)
    {
        Extents = ScaledVector(SubtractVectors(max, min), 0.5f);
        Center = AddVectors(min, Extents);
    }

    /// <summary>
    ///   <para>Grows the Bounds to include the point.</para>
    /// </summary>
    public void Encapsulate(Vector3 point)
    {
        SetMinMax(MinVector(Min, point), MaxVector(Max, point));
    }

    /// <summary>
    ///   <para>Grow the bounds to encapsulate the bounds.</para>
    /// </summary>
    public void Encapsulate(BoundingBox bounds)
    {
        Encapsulate(bounds.Min);
        Encapsulate(bounds.Max);
    }

    /// <summary>
    ///   <para>Does another bounding box intersect with this bounding box?</para>
    /// </summary>
    public bool Intersects(BoundingBox bounds)
    {
        return Min.x <= bounds.Max.x && Max.x >= bounds.Min.x &&
               Min.y <= bounds.Max.y && Max.y >= bounds.Min.y &&
               Min.z <= bounds.Max.z && Max.z >= bounds.Min.z;
    }

    public override string ToString()
    {
        return $"CENTER: {Center} EXTENTS: {Extents}";
    }
}