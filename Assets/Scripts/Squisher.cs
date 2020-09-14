using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Squisher : MonoBehaviour
{
    public bool isTriggered;

    public float squishSpeed = 1f;
    public float retractSpeed = 1f;

    public Transform startPos, endPos;
    public Transform squisherObject;
    public Transform squisherArm;
    public GameObject squishedObject;


    private bool _isSquishing;
    private bool _isRetracted;
    private float _squisherSize;



    // Start is called before the first frame update
    void Start()
    {
        _squisherSize = squisherObject.localScale.y / 2;

    }

    private void Update()
    {
        if (_isSquishing)
        {
            Squish();
        }
        else {
            Retract();
        }
    }


    public void Squish() {
        squisherObject.transform.position += Vector3.down * squishSpeed * Time.deltaTime;

        ScaleArm(Vector3.up, squishSpeed);

        if (squisherObject.position.y <= endPos.position.y + _squisherSize) {
            _isSquishing = false;
            _isRetracted = false;
        }
    }
    void Retract() {
            


        if (squisherObject.position.y >= startPos.position.y - _squisherSize)
        {
            _isRetracted = true;
            if (!isTriggered)
            {
                _isSquishing = true;
            }
        }
        else {
            squisherObject.transform.position += Vector3.up * retractSpeed * Time.deltaTime;

            ScaleArm(Vector3.down, retractSpeed);
        }
    }


    void ScaleArm(Vector3 dir, float speed) {
        Vector3 armScale = squisherArm.localScale;
        armScale += dir * speed * Time.deltaTime;
        squisherArm.localScale = armScale;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            FlattenObject(collision.gameObject.GetComponent<SpriteRenderer>().color, collision.gameObject);
        }
    }

    void FlattenObject(Color objectColor, GameObject go) {
       
        GameObject newObject = Instantiate(squishedObject, go.transform.position, Quaternion.identity) as GameObject;
        newObject.transform.parent = go.transform.parent;
        newObject.name = "Squished " + go.name;
        newObject.GetComponent<SpriteRenderer>().color = objectColor;   
        Destroy(go);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered)
        {

            if (_isRetracted)
            {
                _isSquishing = true;
            }
        }
    }




}
