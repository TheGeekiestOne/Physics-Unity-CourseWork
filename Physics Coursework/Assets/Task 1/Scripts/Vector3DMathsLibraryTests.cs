using UnityEngine;
using static Vector3DMaths;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 * 
 * Tests for the physics calculations
 */
public class Vector3DMathsLibraryTests : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vA = new Vector3(1, 2, 3);
        var vB = new Vector3(5, 7, 8);

        Debug.Log($"Testing Vector Addition {vA} + {vB}");
        Debug.Log($"Result: {AddVectors(vA, vB)}");

        Debug.Log($"Testing Vector Subtraction {vA} - {vB}");
        Debug.Log($"Result: {SubtractVectors(vA, vB)}");

        Debug.Log($"Testing Vector Scaling {vA} to 5");
        Debug.Log($"Result: {ScaledVector(vA, 5)}");

        Debug.Log($"Testing Vector Dot Product {vA} . {vB}");
        Debug.Log($"Result: {VectorDotProduct(vA, vB)}");

        Debug.Log($"Testing Vector Cross Product {vA} x {vB}");
        Debug.Log($"Result: {VectorCrossProduct(vA, vB)}");

        Debug.Log($"Testing Vector Normalize {vA}");
        Debug.Log($"Result: {NormalizedVector(vA)}");

        Debug.Log($"Testing Vector Magnitude {vA}");
        Debug.Log($"Result: {VectorMagnitude(vA)}");

        Debug.Log($"Testing Vector Unit Direction {vA} and {vB}");
        Debug.Log($"Result: {VectorUnitDirection(vA, vB)}");

        Debug.Log($"Testing Vector Distance {vA} and {vB}");
        Debug.Log($"Result: {VectorDistance(vA, vB)}");

        Debug.Log($"Testing Points Inside Radius p1:{vA} p2:{vB} r:4");
        Debug.Log($"Result: {PointsInsideRadius(vA, vB, 4)}");

        var refN = new Vector3(1, 1, 0); // The normal of the surface
        var refInitDir = new Vector3(-1, -0.5f, 0); // The direction of the initial reflection vector

        Debug.Log($"Testing Reflection Vector (NOT-ALIGNED) Normal:{refN} and Init Ref:{refInitDir}");
        Debug.Log($"Result: {ReflectionVector(refN, refInitDir)}");

        Debug.Log($"Testing Reflection Vector (X-ALIGNED) Init Ref:{refInitDir}");
        // since the direction has 0 z axis, we are using the XZ aligned function
        Debug.Log($"Result: {XZAlignedReflectionVector(refInitDir)}"); 

        Debug.Log($"Testing Reflection Vector (Y-ALIGNED) Init Ref:{refInitDir}");
        // since the direction has 0 z axis, we are using the YZ aligned function
        Debug.Log($"Result: {YZAlignedReflectionVector(refInitDir)}"); 
        
        // y = 1/2x+3
        var line = new Line(1/2f, 3);
        var point = new Vector2(-2,2);// Lies on this line
        
        Debug.Log($"Testing Point on line Line:1/2x+3 Point{point}");
        // since the direction has 0 z axis, we are using the YZ aligned function
        Debug.Log($"Result: {IsPointOnBoundedLineXY(line, point)}");
    }
}