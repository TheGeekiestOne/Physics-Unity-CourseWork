using UnityEngine;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public static class Vector3DMaths
{
    //all static class methods as pure utility
    //this deals with 3D vector maths

    /// <summary>
    /// Adds two vectors together
    /// </summary>
    public static Vector3 AddVectors(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x + vec2.x,
            vec1.y + vec2.y,
            vec1.z + vec2.z);
    }

    /// <summary>
    /// Subtracts two vectors
    /// </summary>
    public static Vector3 SubtractVectors(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x - vec2.x,
            vec1.y - vec2.y,
            vec1.z - vec2.z);
    }
    
    /// <summary>
    ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
    /// </summary>
    public static Vector3 MinVector(Vector3 v1, Vector3 v2)
    {
        return new Vector3(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y), Mathf.Min(v1.z, v2.z));
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
    /// </summary>
    public static Vector3 MaxVector(Vector3 v1, Vector3 v2)
    {
        return new Vector3(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z));
    }

    /// <summary>
    /// Returns the dot product of two vectors
    /// </summary>
    public static float VectorDotProduct(Vector3 vec1, Vector3 vec2)
    {
        var xProd = vec1.x * vec2.x;
        var yProd = vec1.y * vec2.y;
        var zProd = vec1.z * vec2.z;
        return xProd + yProd + zProd;
    }
    public static Vector3 VectorCrossProduct(Vector3 v1, Vector3 v2)
    {
        return new Vector3(
            v1.y * v2.z - v1.z * v2.y,
            v1.z * v2.x - v1.x * v2.z,
            v1.x *  v2.y - v1.y *v2.x
            );
    }

    /// <summary>
    /// Returns a unit vector in the direction
    /// </summary>
    public static Vector3 NormalizedVector(Vector3 vec)
    {
        var mag = VectorMagnitude(vec);
        if (Mathf.Approximately(mag, 0f)) return vec;
        
        return new Vector3(
            vec.x / mag,
            vec.y / mag,
            vec.z / mag
        );
    }

    /// <summary>
    /// Returns the squared magnitude for the vector
    /// </summary>
    public static float VectorSqrMagnitude(Vector3 v)
    {
        return VectorDotProduct(v, v);
    }
    
    /// <summary>
    /// Returns the magnitude of the vector
    /// </summary>
    public static float VectorMagnitude(Vector3 vec)
    {
        return Mathf.Sqrt(VectorSqrMagnitude(vec));
    }
    
    /// <summary>
    /// Scales the vector's magnitude by the given value
    /// </summary>
    public static Vector3 ScaledVector(Vector3 vec, float scale)
    {
        return new Vector3(vec.x * scale, vec.y * scale, vec.z * scale);
    }

    /// <summary>
    /// Returns the direction vector between two points
    /// </summary>
    public static Vector3 VectorUnitDirection(Vector3 v1, Vector3 v2)
    {
        return NormalizedVector(SubtractVectors(v2, v1));
    }

    /// <summary>
    /// Returns the distance between two points
    /// </summary>
    public static float VectorDistance(Vector3 p1, Vector3 p2)
    {
        return VectorMagnitude(SubtractVectors(p1, p2));
    }

    /// <summary>
    /// Returns the squared distance between two points, faster for comparing distances
    /// </summary>
    public static float VectorSqrDistance(Vector3 p1, Vector3 p2)
    {
        return VectorSqrMagnitude(SubtractVectors(p1, p2));
    }

    /// <summary>
    /// Returns true if the distance between the points is less than or equal to the radius
    /// </summary>
    public static bool PointsInsideRadius(Vector3 p1, Vector3 p2, float radius)
    {
        // Using sqr distance here to avoid doing sqrt, which is very expensive
        return VectorSqrDistance(p1, p2) < radius * radius;
    }

    /// <summary>
    /// Returns a vector with (0, 0, 0) components
    /// </summary>
    public static Vector3 ZeroVector()
    {
        return new Vector3();
    }

    /// <summary>
    /// Returns the reflection vector, given direction and surface normal
    /// </summary>
    public static Vector3 ReflectionVector(Vector3 normalVector, Vector3 dirVector)
    {
        normalVector = NormalizedVector(normalVector);
        
        // Dir - (2 * <Dir, Normal>) * Normal
        return SubtractVectors(dirVector, ScaledVector(normalVector, 2 * VectorDotProduct(dirVector, normalVector)));
    }

    public static Vector3 XZAlignedReflectionVector(Vector3 dir)
    {
        return new Vector3(dir.x, -dir.y, dir.z);
    }
    
    public static Vector3 XYAlignedReflectionVector(Vector3 dir)
    {
        return new Vector3(dir.x, dir.y, -dir.z);
    }
    
    public static Vector3 YZAlignedReflectionVector(Vector3 dir)
    {
        return new Vector3(-dir.x, dir.y, dir.z);
    }

    /// <summary>
    /// Is point on a line bounded with minX and maxX
    /// </summary>
    public static bool IsPointOnBoundedLineXY(Line line, Vector3 point, float minX = -float.MaxValue, float maxX = float.MaxValue)
    {
        return point.x >= minX && point.x <= maxX && line.PointOnLineXY(point);
    }
    
    /// <summary>
    /// Is point on a line bounded with minX and maxX
    /// </summary>
    public static bool IsPointOnBoundedLineXZ(Line line, Vector3 point, float minX = -float.MaxValue, float maxX = float.MaxValue)
    {
        return point.x >= minX && point.x <= maxX && line.PointOnLineXZ(point);
    }
    
}