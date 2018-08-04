using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 axis;

    // Update is called once per frame
    void Update()
    {
        //float addToAngle = 0.0f;
        //transform.Rotate(axis, addToAngle + speed);
        transform.RotateAround(Vector3.zero, axis, speed * Time.deltaTime);
    }
}
