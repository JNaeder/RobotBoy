using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squisher : MonoBehaviour
{

    public float squishSpeed = 1f;
    public float retractSpeed = 1f;

    public Transform startPos, endPos;
    public Transform squisherObject;
    public Transform squisherArm;

    public bool isSquishing;

    float squisherSize;
    float squisherArmStartSize;



    // Start is called before the first frame update
    void Start()
    {
        squisherSize = squisherObject.localScale.y / 2;
        squisherArmStartSize = squisherArm.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSquishing) {
            Squish();

        } else {
            Retract();

        }

        
    }


    void Squish() {
        squisherObject.transform.position += Vector3.down * squishSpeed * Time.deltaTime;

        Vector3 armScale = squisherArm.localScale;
        armScale += Vector3.up * squishSpeed * Time.deltaTime;
        squisherArm.localScale = armScale;

        if (squisherObject.position.y <= endPos.position.y + squisherSize) {
            isSquishing = false;
        }

    }

    void Retract() {
        squisherObject.transform.position += Vector3.up * retractSpeed * Time.deltaTime;

        Vector3 armScale = squisherArm.localScale;
        armScale += Vector3.down * retractSpeed * Time.deltaTime;
        squisherArm.localScale = armScale;


        if (squisherObject.position.y >= startPos.position.y - squisherSize)
        {
            isSquishing = true;
        }


    }
}
