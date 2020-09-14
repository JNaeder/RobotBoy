using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Main : MonoBehaviour
{   //--Robot Stuff--
    Robot_Movement movement;
    Robot_Throwing throwing;
    Robot_Teleporting teleporting;


    //--Other Stuff--
    [HideInInspector]public Animator anim;
    [HideInInspector]public Rigidbody2D rB;


    //--Initla States--
    [HideInInspector]public Vector3 startBodyPos, startHeadPos;


    //--Different States Of the Throwing Mechanic--
    public enum RobotState { moving, throwing, detached, teleporting, dead }
    public RobotState currentRobotState = RobotState.moving;


    //--Transform of the body and the head--
    public Transform bodyTrans, headTrans, headParent, armSprite, headHolder;
    //--The rigidbody of the head--
    public Rigidbody2D headRB;

    private void Start()
    {
        //Set Robot Scripts
        movement = GetComponent<Robot_Movement>();
        throwing = GetComponent<Robot_Throwing>();
        teleporting = GetComponent<Robot_Teleporting>();

        // Set Things Up
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startBodyPos = bodyTrans.localPosition;
        startHeadPos = headTrans.localPosition;

    }

    private void Update()
    {
        SetStateStatus();
        teleporting.RechargeTeleportPower();
    }

    private void SetStateStatus()
    {
        // Control the states of the Robot
        if (currentRobotState == RobotState.moving)
        {
            movement.Moving();
            throwing.ThrowButtonCheck();
        }
        else if (currentRobotState == RobotState.throwing)
        {
            throwing.Throwing();
        }
        else if (currentRobotState == RobotState.detached)
        {
            StartCoroutine(teleporting.Detached());
            movement.Moving();
        }
        else if (currentRobotState == RobotState.dead) {

            //Death Stuff
        }
    }
}
