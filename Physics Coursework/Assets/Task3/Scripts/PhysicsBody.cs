using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vector3DMaths;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public sealed class PhysicsBody : MonoBehaviour
{
    private PhysicsShape _attachedShape;

    [SerializeField] private float _mass;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float _friction;
    public ulong Identifier { get; private set; }

    public BoundingBox Bounds => _attachedShape.GetBounds();

    public Vector3 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public float Mass
    {
        get => _mass;
        set => _mass = value;
    }

    public bool Static => _attachedShape.IsStatic();

    private void OnEnable()
    {
        Identifier = PhysicsManager.Instance.RegisterPhysicsBody(this);
    }

    private void OnDisable()
    {
        if (PhysicsManager.Instance != null)
            PhysicsManager.Instance.UnregisterPhysicsBody(this);
    }

    public void PhysicsUpdate()
    {
        if (Static) return;

        _velocity = AddVectors(_velocity, ScaledVector(NormalizedVector(_velocity), -_friction * Time.deltaTime));
        transform.position = AddVectors(transform.position, ScaledVector(_velocity, Time.deltaTime));
    }

    public void AttachShape(PhysicsShape shape)
    {
        _attachedShape = shape;
    }

    public void DetachShape(PhysicsShape shape)
    {
        _attachedShape = shape;
    }

    public bool IsCollidingWith(PhysicsBody body)
    {
        return body.IsCollidingWith(_attachedShape);
    }

    private bool IsCollidingWith(PhysicsShape s)
    {
        return _attachedShape.IsCollidingWith(s);
    }

    public void ResolveCollisions(PhysicsBody body)
    {
        if (body.Static) return;
        body.ResolveCollisions(_attachedShape);
    }

    private void ResolveCollisions(PhysicsShape other)
    {
        print(name);
        _attachedShape.HandleOverlap(other);

        // If there is no velocity, we dont want to handle dynamic collision
        if (Mathf.Approximately(VectorSqrMagnitude(_velocity) + VectorSqrMagnitude(other.AttachedPhysicsBody.Velocity), 0f))
            return;

        // If the other body is static
        if (other.AttachedPhysicsBody.Static)
        {
            var selfPosition = transform.position;
            var collisionPoint = other.ClosestPointAndNormalOnShape(selfPosition, out var normal);
            _velocity = ReflectionVector(normal, _velocity);
        }
        else
        {
            var otherBody = other.AttachedPhysicsBody;
            var newV = GetCollisionVelocityForMovingBalls(this, otherBody);
            var otherNewV = GetCollisionVelocityForMovingBalls(otherBody, this);
            Velocity = newV;
            otherBody.Velocity = otherNewV;
        }
    }

    private Vector3 GetCollisionVelocityForMovingBalls(PhysicsBody p1, PhysicsBody p2)
    {
        var x1 = p1.transform.position;
        x1.y = 0;
        var x2 = p2.transform.position;
        x2.y = 0;

        var m1 = p1.Mass;
        var m2 = p2.Mass;

        var v1 = p1.Velocity;
        var v2 = p2.Velocity;
        
        var v1MinusV2 = SubtractVectors(v1, v2);
        var x1MinusX2 = SubtractVectors(x1, x2);
        var dot = VectorDotProduct(v1MinusV2, x1MinusX2);
        var sqrMag = VectorSqrMagnitude(x1MinusX2);
        var m = (2*m2)/(m1+m2);
        
        var newV = SubtractVectors(v1, ScaledVector(x1MinusX2, m * (dot / sqrMag)));
        return newV;
    }
}