using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Teleporting : MonoBehaviour
{
    //--Robot Stuff--
    Robot_Main main;
    Robot_Throwing throwing;

    //--Other Stuff--
    public GameObject teleportPS;

    // Stats for the recharge teleport
    public float maxTeleportPower = 10;
    public float currentTeleportPower;
    [Range(1, 20)]
    public float teleportRechargeRate = 1;


    // Start is called before the first frame update
    void Start()
    {
        //Set Up Robot Stuff
        main = GetComponent<Robot_Main>();
        throwing = GetComponent<Robot_Throwing>();

        //Set Up Teleporting power
        currentTeleportPower = maxTeleportPower;
    }

    public IEnumerator Detached()
    {
        yield return new WaitForSeconds(0.5f);
        throwing.CheckForHead();
        
        if (Input.GetMouseButtonDown(0))
        {
            //Check if teleport power has recharged. Can't teleport unless it's charged.
            if (currentTeleportPower >= maxTeleportPower)
            {
                main.headTrans.parent = main.headParent;
                Teleport();
                main.currentRobotState = Robot_Main.RobotState.teleporting;
                yield return new WaitForSeconds(0.2f);
                main.currentRobotState = Robot_Main.RobotState.moving;
            }
        }
        // Reset back to body position. "Redo"
        else if (Input.GetMouseButtonDown(1))
        {
            main.headTrans.parent = transform;
            Reset();
            main.currentRobotState = Robot_Main.RobotState.teleporting;
            yield return new WaitForSeconds(0.2f);
            main.currentRobotState = Robot_Main.RobotState.moving;
        }
    }

    void Teleport()
    {
        //Use Teleporting Power
        currentTeleportPower = 0;

        //Use half of the heads velocity
        main.rB.velocity = main.headRB.velocity * 0.75f;
        Vector2 headVel = main.headRB.velocity;

        //Play the particle system for teleporting. And delete it once it's finished playing.
        GameObject pS = Instantiate(teleportPS, main.headTrans.position, Quaternion.identity) as GameObject;
        Destroy(pS, pS.GetComponent<ParticleSystem>().main.duration);

        //Move body To head position and reset everything.
        transform.position = main.headTrans.position;
        Reset();

        //Turn the player toward the direction of throwing head
        if (headVel.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (headVel.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Reset()
    {
        // Reset the head back to the body. "Undo"
        main.headRB.bodyType = RigidbodyType2D.Kinematic;
        main.headRB.velocity = Vector2.zero;
        main.headRB.angularVelocity = 0;
        main.bodyTrans.localPosition = main.startBodyPos;
        main.headTrans.localPosition = main.startHeadPos;
        main.headTrans.localScale = main.bodyTrans.localScale;
        main.headTrans.rotation = Quaternion.Euler(Vector3.zero);
        main.headTrans.GetComponent<DeathDetector>().enabled = false;
    }

    // Increasing the teleport power back to max. Over recharge rate time
    public void RechargeTeleportPower()
    {
        if (currentTeleportPower < maxTeleportPower)
        {
            currentTeleportPower += teleportRechargeRate * Time.deltaTime;
        }
    }
}