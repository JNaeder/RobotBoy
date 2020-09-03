using System.Collections;
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
    Vector2 throwVector;

    Camera cam;
    Rigidbody2D rB;
    public Trajectory trajectory;


    public Rigidbody2D headRB;
    public float trajectoryMin, trajectoryMax;

    private void Start()
    {
        cam = Camera.main;
        rB = GetComponent<Rigidbody2D>();

        startBodyPos = bodyTrans.localPosition;
        startHeadPos = headTrans.localPosition;

    }

    private void Update()
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


            Debug.DrawLine(startMousePos, currentMousePos);

            
            throwVector = startMousePos - currentMousePos;
            if (throwVector.magnitude > trajectoryMin)
            {
                trajectory.Show();
                trajectory.UpdateDots(headTrans.position, throwVector);
            }
        }

        if (Input.GetMouseButtonUp(0)) {

            ThrowHead(throwVector);

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


    void ThrowHead(Vector2 throwVector) {
        //Debug.Log("Throw");

        trajectory.Hide();
        headRB.bodyType = RigidbodyType2D.Dynamic;
        headRB.AddForce(throwVector * throwPower, ForceMode2D.Impulse);
        //Debug.Log(throwVector);

    }

    void Teleport() {
        //Debug.Log("Teleport!");

        rB.velocity = headRB.velocity;

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
