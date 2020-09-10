using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Squisher : MonoBehaviour
{

    public float squishSpeed = 1f;
    public float retractSpeed = 1f;
    public Transform startPos, endPos;
    public Transform squisherObject;
    public Transform squisherArm;


    private bool _isSquishing;
    private float _squisherSize;



    // Start is called before the first frame update
    void Start()
    {
        _squisherSize = squisherObject.localScale.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSquishing) {
            Squish();

        } else {
            Retract();
        }
    }
    void Squish() {
        squisherObject.transform.position += Vector3.down * squishSpeed * Time.deltaTime;

        ScaleArm(Vector3.up, squishSpeed);

        if (squisherObject.position.y <= endPos.position.y + _squisherSize) {
            _isSquishing = false;
        }
    }
    void Retract() {
        squisherObject.transform.position += Vector3.up * retractSpeed * Time.deltaTime;

        ScaleArm(Vector3.down, retractSpeed);


        if (squisherObject.position.y >= startPos.position.y - _squisherSize)
        {
            _isSquishing = true;
        }
    }
    void ScaleArm(Vector3 dir, float speed) {
        Vector3 armScale = squisherArm.localScale;
        armScale += dir * speed * Time.deltaTime;
        squisherArm.localScale = armScale;
    }
}
