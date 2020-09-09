using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
    public float rotateSpeed;
    public bool rotateDirection;
    public Transform wallObject;


    // Update is called once per frame
    void Update()
    {
        if (rotateDirection)
        {
            wallObject.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
        }
        else {
            wallObject.Rotate(new Vector3(0,0,-rotateSpeed * Time.deltaTime));
        }
    }
}
