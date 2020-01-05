using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */

[DefaultExecutionOrder(-100)]
public sealed class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance { get; private set; }

    private readonly Dictionary<ulong, PhysicsBody> _pBodies = new Dictionary<ulong, PhysicsBody>();

    [SerializeField] private Vector3 _gravity;

    public Vector3 Gravity => _gravity;

    private bool _initialized;
    private ulong _pBodyIdCounter = 1;

    private void InitializeInstance()
    {
        if (_initialized) return;

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        _initialized = true;
    }

    private void Awake()
    {
        InitializeInstance();
    }

    public ulong RegisterPhysicsBody(PhysicsBody pBody)
    {
        print($"Attached pBody: {pBody.name}");
        _pBodies.Add(_pBodyIdCounter, pBody);
        return _pBodyIdCounter++;
    }

    public void UnregisterPhysicsBody(PhysicsBody pBody)
    {
        if (pBody.Identifier == 0) return;
        _pBodies.Remove(pBody.Identifier);
    }

    private void FixedUpdate()
    {
        var pBodies = _pBodies.Values;

        // Static collisions i.e position displacement if overlapping
        foreach (var pBody in pBodies)
        {
            pBody.PhysicsUpdate();
            
            foreach (var otherBody in pBodies)
            {
                // we continue If,
                // 1. we're testing ourselves
                // 2. our bounding box doesnt collide with the other physics body
                // 3. we aren't actually colliding
                if (otherBody == pBody) continue;
                
                 //print($"pBodyBoundsIntersect:{pBody.Bounds.Intersects(otherBody.Bounds)} pBodyColliding:{pBody.IsCollidingWith(otherBody)} pBody: {pBody} otherBody: {otherBody}");
                if (pBody.Bounds.Intersects(otherBody.Bounds) && pBody.IsCollidingWith(otherBody))
                {
                    // We most definitely have a collision
                    pBody.ResolveCollisions(otherBody);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var pBody in _pBodies.Values)
        {
            Gizmos.DrawWireCube(pBody.Bounds.Center, pBody.Bounds.Size);
        }

        Gizmos.color = Color.white;
    }
}