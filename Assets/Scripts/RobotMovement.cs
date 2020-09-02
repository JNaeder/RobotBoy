﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public float movementSpeed = 3f;
    public float throwPower = 50f;

    public enum RobotState {moving, throwing, detached, teleporting}
    public RobotState currentRobotState = RobotState.moving;


    Vector3 startMousePos, currentMousePos;
    public Transform bodyTrans, headTrans;
    Vector3 startBodyPos, startHeadPos;

    Camera cam;


    public Rigidbody2D headRB;
    public GameObject arrowObject;

    private void Start()
    {
        cam = Camera.main;
        startBodyPos = bodyTrans.localPosition;
        startHeadPos = headTrans.localPosition;
        arrowObject.SetActive(false);

    }

    private void FixedUpdate()
    {


        if (currentRobotState == RobotState.moving)
        {
            Movement();
        }
        else if (currentRobotState == RobotState.throwing) {
            Throwing();
        }
        else if (currentRobotState == RobotState.detached) {
            StartCoroutine(Detached());
        }
    }


    void Movement() {
        float h = Input.GetAxis("Horizontal");
        transform.position += new Vector3(h, 0, 0) * movementSpeed * Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            currentRobotState = RobotState.throwing;
            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void Throwing() {
        if (Input.GetMouseButton(0)) {
            //Debug.Log("Holding");
            currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            arrowObject.SetActive(true);
            Vector3 diff = startMousePos - currentMousePos;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            arrowObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        }

        if (Input.GetMouseButtonUp(0)) {

            ThrowHead();

            currentRobotState = RobotState.detached;
        }
    }

    IEnumerator Detached() {
        if (Input.GetMouseButtonDown(0))
        {

            Teleport();

            currentRobotState = RobotState.teleporting;

            yield return new WaitForSeconds(0.2f);

            currentRobotState = RobotState.moving;
        }
        else if (Input.GetMouseButtonDown(1)) {

            Reset();
            currentRobotState = RobotState.teleporting;

            yield return new WaitForSeconds(0.2f);

            currentRobotState = RobotState.moving;
        }

        
    }


    void ThrowHead() {
        //Debug.Log("Throw");

        arrowObject.SetActive(false);
        Vector3 throwVector = startMousePos - currentMousePos;
        headRB.bodyType = RigidbodyType2D.Dynamic;
        headRB.AddForce(throwVector * throwPower);
        //Debug.Log(throwVector);

    }

    void Teleport() {
        //Debug.Log("Teleport!");
       
        headRB.bodyType = RigidbodyType2D.Kinematic;
        headRB.velocity = Vector2.zero;
        transform.position = headTrans.position;
        bodyTrans.localPosition = startBodyPos;
        headTrans.localPosition = startHeadPos;
        

    }

    void Reset() {
        //Debug.Log("Reset");

        headRB.bodyType = RigidbodyType2D.Kinematic;
        headRB.velocity = Vector2.zero;
        bodyTrans.localPosition = startBodyPos;
        headTrans.localPosition = startHeadPos;


    }


}
