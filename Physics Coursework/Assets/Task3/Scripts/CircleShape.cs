using System;
using UnityEditor;
using UnityEngine;
using static Vector3DMaths;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public class CircleShape : PhysicsShape
{
    [SerializeField] private float _radius;

    public float Radius
    {
        get => _radius;
        set => _radius = value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        GizmoCircle(new Vector3(transform.position.x, 0, transform.position.z), _radius, 30);
        Gizmos.color = Color.white;
    }
    
    private void GizmoCircle(Vector3 center, float radius, int resolution)
    {
        var angle = (2 * Mathf.PI) / resolution;
        for (int i = 0; i < resolution; i++)
        {
            var from = RadToVector2(angle * i, radius);
            var to = RadToVector2(angle * (i % resolution + 1), radius);
            Gizmos.DrawLine(AddVectors(from, center), AddVectors(to, center));
        }
    }

    private Vector3 RadToVector2(float angle, float radius = 1f)
    {
        return new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle)) * radius;
    }
    
    public override BoundingBox GetBounds()
    {
        var diameter = 2 * _radius;
        return new BoundingBox(transform.position, new Vector3(diameter, diameter, diameter));
    }

    public override bool IsStatic()
    {
        return false;
    }

    public override bool IsPointInsideShape(Vector3 point)
    {
        return VectorSqrDistance(transform.position, point) <= _radius * _radius;
    }

    public override bool IsCollidingWith(PhysicsShape shape)
    {
        var position = transform.position;
        return PointsInsideRadius(position, shape.ClosestPointInsideShape(position), _radius);
    }

    public override Vector3 ClosestPointInsideShape(Vector3 point)
    {
        var position = transform.position;

        // If the point given is already inside the sphere, return it
        // else return the closest point on the sphere
        return PointsInsideRadius(position, point, _radius) ? point : ClosestPointOnShape(point);
    }

    public override Vector3 ClosestPointOnShape(Vector3 point)
    {
        var position = transform.position;
        return AddVectors(position, ScaledVector(VectorUnitDirection(position, point), _radius));
    }

    public override void HandleOverlap(PhysicsShape shape)
    {
        // For a visual representation I implemented the whole thing on Desmos
        // https://www.desmos.com/calculator/r7cdvkyos2
        // Note: the implementation there does not support shape going inside other shape 
        var selfPosition = transform.position;
        
        // The closest point to this shape on the surface of other shape
        var closestPointToSelfOnOtherShape = shape.ClosestPointOnShape(selfPosition);
        
        // Is our origin inside the other shape?
        var isSelfInsideOtherShape =
            PointsInsideRadius(shape.ClosestPointInsideShape(selfPosition), selfPosition, 0.001f);
        
        // The closest point to `closestPointToSelfOnOtherShape` that lies on the surface of this shape
        var closestPointToOtherShapeOnSelf = ClosestPointOnShape(isSelfInsideOtherShape
            ? SubtractVectors(selfPosition, closestPointToSelfOnOtherShape)
            : closestPointToSelfOnOtherShape);
        
        // The amount of displacement opposite to the other shape
        var displacement = ScaledVector(
            NormalizedVector(SubtractVectors(closestPointToSelfOnOtherShape, closestPointToOtherShapeOnSelf)),
            VectorMagnitude(SubtractVectors(closestPointToSelfOnOtherShape, closestPointToOtherShapeOnSelf)));
        
        // we dont want to move the other body since its static
        if (!AttachedPhysicsBody.Static && shape.AttachedPhysicsBody.Static)
        {
            var pos = AddVectors(selfPosition, displacement);
            print(pos);
            AttachedPhysicsBody.transform.position = pos;
        }
        
        // we want to move both the bodies equally in opposite directions
        else if (!AttachedPhysicsBody.Static && !shape.AttachedPhysicsBody.Static)
        {
            var halfDistance = ScaledVector(displacement, 0.5f);
            transform.position = AddVectors(selfPosition, halfDistance);
            shape.AttachedPhysicsBody.transform.position =
                SubtractVectors(shape.AttachedPhysicsBody.transform.position, halfDistance);
        }
    }

    public override Vector3 ClosestPointAndNormalOnShape(Vector3 point, out Vector3 normal)
    {
        normal = Vector3.zero;
        return Vector3.zero;
    }
}