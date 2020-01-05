using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vector3DMaths;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 * 
 * Bouncing ball
 */
public class BallBounce : MonoBehaviour
{
    [SerializeField] private Vector3 _initialVelocity;
    [SerializeField] private Vector3 _gravity;
    [SerializeField] private float _e;

    private void FixedUpdate()
    {
        transform.position = AddVectors(transform.position, ScaledVector(_initialVelocity, Time.deltaTime));
        _initialVelocity = AddVectors(_initialVelocity, ScaledVector(_gravity, Time.deltaTime));
        if (transform.position.y <= 0 && _initialVelocity.y < 0)
        {
            _initialVelocity = ScaledVector(XZAlignedReflectionVector(_initialVelocity), _e * _e);
        }
    }
}
