using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Throwing : MonoBehaviour
{   //--Robot Stuff--
    Robot_Main main;

    //--Other Stuff--
    Camera cam;

    //--Initial Stuff--
    Vector3 startMousePos, currentMousePos;
    Vector2 throwVector;

    //--Public stuff--
    // Min and Max Trajectory Magnitudes
    public float trajectoryMin, trajectoryMax;
    public Trajectory trajectory;


    private void Start()
    {   // Set Up Robot Stuff
        main = GetComponent<Robot_Main>();

        //Set Up Other Stuff
        cam = Camera.main;
    }

    //Chceck to see if throw button has been pressed;
    public void ThrowButtonCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            main.currentRobotState = Robot_Main.RobotState.throwing;
            // Get the start mouse position
            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    //Throwing State
    public void Throwing()
    {
        if (Input.GetMouseButton(0))
        {

            //Get the current mouse position and keep updating it
            currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            //Set Arm rotation
            SetThrowingArmAngle(currentMousePos);

            //Set the head onto the arm for throwing
            main.headTrans.parent = main.headHolder;
            main.headTrans.localRotation = Quaternion.Euler(new Vector3(0, 0, -180));
            main.headTrans.localPosition = new Vector3(0, -0.4f, 0);

            //Clamping the throw vector to the maximum trajectory
            throwVector = Vector3.ClampMagnitude(startMousePos - currentMousePos, trajectoryMax);

            //Hiding the trajectory if magnitude is under mimimum trajectory
            if (throwVector.magnitude > trajectoryMin)
            {
                trajectory.Show();
                trajectory.UpdateDots(main.headTrans.position, throwVector);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Throw the head and change state to detached
           ThrowHead(throwVector);
           main.currentRobotState = Robot_Main.RobotState.detached;
           ResetArmPosition();
        }
        if (Input.GetMouseButtonDown(1))
        {
            //Cancel the throwing, and go back to normal moving
            main.currentRobotState = Robot_Main.RobotState.moving;
            trajectory.Hide();
            ResetArmPosition();
            ResetHeadPosition();
        }
    }

    void SetThrowingArmAngle(Vector3 currentMousePos)
    {
        Vector3 diff = startMousePos - currentMousePos;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (diff.x > 0)
        {
            //armSprite.localRotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (diff.x < 0)
        {
            //armSprite.localRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    //Throw the head
    void ThrowHead(Vector2 throwVector)
    {
        //Hide the trajectory efefct, and throw the head. Use the throw vector to throw the head in that direction. Remove it from the parent transform
        trajectory.Hide();
        main.headRB.bodyType = RigidbodyType2D.Dynamic;
        main.headRB.AddForce(throwVector, ForceMode2D.Impulse);
        main.headRB.AddTorque(Mathf.Clamp(throwVector.x, -2f, 2f) * -throwVector.magnitude);
        main.headTrans.parent = null;

    }
    //Put the arm back
    void ResetArmPosition()
    {
        main.armSprite.localRotation = Quaternion.Euler(Vector3.zero);
    }

    //Put Head Back
    void ResetHeadPosition()
    {
        main.headRB.bodyType = RigidbodyType2D.Kinematic;
        main.headRB.velocity = Vector2.zero;
        main.headRB.angularVelocity = 0;
        main.headTrans.parent = main.headParent;
        main.headTrans.localScale = main.bodyTrans.localScale;
        main.headTrans.localRotation = Quaternion.Euler(Vector3.zero);
        main.headTrans.localPosition = main.startHeadPos;
    }
}
