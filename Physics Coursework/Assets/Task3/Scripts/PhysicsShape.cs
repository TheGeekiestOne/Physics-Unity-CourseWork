using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public abstract class PhysicsShape : MonoBehaviour
{
    public PhysicsBody AttachedPhysicsBody { get; protected set; }

    protected virtual void Awake()
    {
        AttachedPhysicsBody = GetComponentInParent<PhysicsBody>();
        AttachedPhysicsBody.AttachShape(this);
    }

    protected virtual void OnDestroy()
    {
        AttachedPhysicsBody.DetachShape(this);
    }

    public abstract BoundingBox GetBounds();
    public abstract bool IsPointInsideShape(Vector3 point);
    public abstract bool IsCollidingWith(PhysicsShape shape);
    public abstract Vector3 ClosestPointInsideShape(Vector3 point);
    public abstract Vector3 ClosestPointOnShape(Vector3 point);
    public abstract Vector3 ClosestPointAndNormalOnShape(Vector3 point, out Vector3 normal);
    public abstract void HandleOverlap(PhysicsShape shape);

    public abstract bool IsStatic();
}
