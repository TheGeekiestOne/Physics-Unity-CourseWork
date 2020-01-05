using System;
using UnityEngine;
using UnityEngine.Serialization;
using static Vector3DMaths;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public class SquareShape : PhysicsShape
{
    [SerializeField] private Vector2 _size;

    public Vector2 Size => _size;

    public Vector3[] Verts
    {
        get
        {
            var position = new Vector3(transform.position.x, 0, transform.position.z);
            var eulerAngles = transform.eulerAngles;
            return new[]
            {
                AddVectors(RotateVectorXZ(new Vector3(-Size.x, 0, -Size.y), eulerAngles.y), position), // bottom left
                AddVectors(RotateVectorXZ(new Vector3(-Size.x, 0, Size.y), eulerAngles.y), position), // top left
                AddVectors(RotateVectorXZ(new Vector3(Size.x, 0, Size.y), eulerAngles.y), position), // top right
                AddVectors(RotateVectorXZ(new Vector3(Size.x, 0, -Size.y), eulerAngles.y), position), // bottom right
            };
        }
    }

    private void OnDrawGizmos()
    {
        var v = Verts;
        Gizmos.color = Color.green;
        
        for (int i = 0; i < v.Length; i++)
        {
            var next = i + 1 >= v.Length ? 0 : i + 1;
            Gizmos.DrawLine(v[i], v[next]);
        }
        
        Gizmos.color = Color.white;
    }

    private Vector3 RotateVectorXZ(Vector3 p, float angle)
    {
        //x2=cosβx1−sinβy1
        //y2=sinβx1+cosβy1
        var rad = -angle * Mathf.Deg2Rad;

        var cos = Mathf.Cos(rad);
        var sin = Mathf.Sin(rad);
        return new Vector3(cos * p.x - sin * p.z, 0, sin * p.x + cos * p.z);
    }

    public override BoundingBox GetBounds()
    {
        var position = transform.position;
        var b = new BoundingBox(position, Size);
        foreach (var vert in Verts)
        {
            b.Encapsulate(vert);
        }

        return b;
    }

    public float AreaOfPoints(Vector3 a, Vector3 b, Vector3 c)
    {
        // Area for triangles using its points
        // A = (x1y2 + x2y3 + x3y1 – x1y3 – x2y1 – x3y2)/2
        return (a.x * b.z + b.x * c.z + c.x * a.z - a.x * c.z - b.x * a.z - c.x * b.z) / 2;
    }

    public override bool IsPointInsideShape(Vector3 point)
    {
        var v = Verts;
        // Check this answer for an explanation
        // https://math.stackexchange.com/a/190117
        var area = (Size.x * 2) * (Size.y * 2);
        var apb = Mathf.Abs(AreaOfPoints(v[0], point, v[1]));
        var bpc = Mathf.Abs(AreaOfPoints(v[1], point, v[2]));
        var cpd = Mathf.Abs(AreaOfPoints(v[2], point, v[3]));
        var dpa = Mathf.Abs(AreaOfPoints(v[3], point, v[0]));
        return Mathf.Approximately(apb + bpc + cpd + dpa, area);
    }

    public override bool IsCollidingWith(PhysicsShape shape)
    {
        // If the shape we're colliding is static then we dont have to check for collisions
        if (shape.IsStatic()) return false;
        
        var collisionPoint = ClosestPointOnShape(shape.AttachedPhysicsBody.transform.position);
        // since we know we'll only be colliding with circles
        return shape.IsPointInsideShape(collisionPoint);
    }

    public override Vector3 ClosestPointInsideShape(Vector3 point)
    {
        return IsPointInsideShape(point) ? point : ClosestPointOnShape(point);
    }

    public override Vector3 ClosestPointOnShape(Vector3 point)
    {
        var v = Verts;
        var closestPoint = Vector3.zero;

        for (int i = 0; i < v.Length; i++)
        {
            var next = i + 1 >= v.Length ? 0 : i + 1;
            var l1 = new Line(v[i].x, v[i].z, v[next].x, v[next].z);
            var n1 = new Line(-1f / l1.M, point.x, point.z);
            //Gizmos.DrawLine(new Vector3(0, 0, l1.Evaluate(0)), new Vector3(10, 0, l1.Evaluate(10)));
            //Gizmos.DrawLine(new Vector3(0, 0, n1.Evaluate(0)), new Vector3(10, 0, n1.Evaluate(10)));

            //Gizmos.DrawCube(v[i], Vector3.one*0.1f);
            //Gizmos.DrawCube(v[next], Vector3.one*0.1f);
            
            Line.IntersectXZ(l1, n1, out var ip);
            // clamp point
            var clampX = Mathf.Clamp(ip.x, Mathf.Min(v[i].x, v[next].x), Mathf.Max(v[i].x, v[next].x));
            var pointOnLine = new Vector3(clampX, 0, l1.Evaluate(clampX));
            
            //Gizmos.DrawSphere(ip, .1f);
            //return closestPoint;
            if (i == 0 || VectorDistance(point, closestPoint) >= VectorDistance(point, pointOnLine))
                closestPoint = pointOnLine;
        }

        return closestPoint;
    }

    public override Vector3 ClosestPointAndNormalOnShape(Vector3 point, out Vector3 normal)
    {
        var v = Verts;
        var closestPoint = Vector3.zero;
        normal = Vector3.zero;

        for (int i = 0; i < v.Length; i++)
        {
            var next = i + 1 >= v.Length ? 0 : i + 1;
            var l1 = new Line(v[i].x, v[i].z, v[next].x, v[next].z);
            var n1 = new Line(-1f / l1.M, point.x, point.z);

            Line.IntersectXZ(l1, n1, out var ip);
            // clamp point
            var clampX = Mathf.Clamp(ip.x, Mathf.Min(v[i].x, v[next].x), Mathf.Max(v[i].x, v[next].x));
            var pointOnLine = new Vector3(clampX, 0, l1.Evaluate(clampX));

            if (i == 0 || VectorDistance(point, closestPoint) >= VectorDistance(point, pointOnLine))
            {
                var dir = VectorUnitDirection(v[i], v[next]);
                closestPoint = pointOnLine;
                normal = new Vector3(-dir.z,0, dir.x);
            }
        }

        return closestPoint;
    }

    public override void HandleOverlap(PhysicsShape shape)
    {
        // We will not handle any overlapping
    }

    public override bool IsStatic()
    {
        // We are always static
        return true;
    }
}