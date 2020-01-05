using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Created by : Ayran Olckers
 * 01/2020
 * Physics Assignment
 */
public class ShereBehaviourScript : MonoBehaviour
{
    public Vector3 initVelocity;
    private Vector3 spherePosition;

    // Start is called before the first frame update
    void Start()
    {
        spherePosition = new Vector3(0.0f, 0.5f, 0.0f);
        Vector3 modVector = new Vector3(0.0f, 1.0f, 0.0f);

        //this.transform.position = Vector3DMaths.addVectors(spherePosition, modVector);
    }

    // Update is called once per frame
    void Update()
    {
        spherePosition.x = spherePosition.x + 0.05f;
        spherePosition.y = spherePosition.y + 0.05f;
    }
}
