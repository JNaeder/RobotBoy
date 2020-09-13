using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    //----Player Stats----
    //
    // How fast Player moves left and right
    public float movementSpeed = 3f;
    // Transform of the body and the head
    public Transform bodyTrans, headTrans, headParent, armSprite, headHolder;
    // The rigidbody of the head
    public Rigidbody2D headRB;
    // Min and Max Trajectory Magnitudes
    public float trajectoryMin, trajectoryMax;
    // Stats for the recharge teleport
    [HideInInspector]
    public float maxTeleportPower = 10;
    [HideInInspector]
    public float currentTeleportPower;
    [Range(1, 20)]
    public float teleportRechargeRate = 1;

    public LayerMask headLayer;


    // Different States Of the Throwing Mechanic
    public enum RobotState {moving, throwing, detached, teleporting, dead}
    public RobotState currentRobotState = RobotState.moving;

    //Different Ways Of Dying
    public enum DeathMethod {electric, squish};

    //Init positions of mouse and player
    Vector3 startMousePos, currentMousePos;
    Vector3 startBodyPos, startHeadPos;
    Vector2 throwVector;

    float h;
    
    // refrences to camera and player Rigidbody and Animator
    Camera cam;
    Rigidbody2D rB;
    Animator anim;

    //effect prefabs
    public Trajectory trajectory;
    public GameObject teleportPS;

    //Death Prefabs
    public GameObject electricDeath;
    public GameObject squishDeath;

    private void Start()
    {
        //Set up all these things
        cam = Camera.main;
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startBodyPos = bodyTrans.localPosition;
        startHeadPos = headTrans.localPosition;
        currentTeleportPower = maxTeleportPower;
        
    }

    private void Update()
    {

        // Control the states of the Robot
        if (currentRobotState == RobotState.moving)
        {
            Movement();
            ThrowButtonCheck();
        }
        else if (currentRobotState == RobotState.throwing) {
            //Movement();
            Throwing();
        }
        else if (currentRobotState == RobotState.detached) {
            StartCoroutine(Detached());
            Movement();
        }

        //Recharge the teleport Power
        RechargeTeleportPower();
        
        

    }

    //Moving Left And Right
    void Movement() {
        h = Input.GetAxis("Horizontal");
        transform.position += new Vector3(h, 0, 0) * movementSpeed * Time.deltaTime;

        //update the animator wiht H
        anim.SetFloat("h", Mathf.Abs(h));

        //Turn the Player toward direction of movement
        if (h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (h < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
    
    }

    //Chceck to see if throw button has been pressed;
    void ThrowButtonCheck() {
        if (Input.GetMouseButtonDown(0))
        {
            currentRobotState = RobotState.throwing;
            // Get the start mouse position
            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }

    }

    //Throwing State
    void Throwing() {
        if (Input.GetMouseButton(0)) {
            
            // Get the current mouse position and keep updating it
            currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            //Set Arm rotation
            SetThrowingArmAngle(currentMousePos);

            // Set the head onto the arm for throwing
            headTrans.parent = headHolder;
            headTrans.localRotation = Quaternion.Euler(new Vector3(0, 0, -180));
            headTrans.localPosition = new Vector3(0, -0.4f, 0);

            // Clamping the throw vector to the maximum trajectory
            throwVector = Vector3.ClampMagnitude(startMousePos - currentMousePos, trajectoryMax);

            // hiding the trajectory if magnitude is under mimimum trajectory
            if (throwVector.magnitude > trajectoryMin)
            {
                trajectory.Show();
                trajectory.UpdateDots(headTrans.position, throwVector);
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            // Throw the head and change state to detached
            ThrowHead(throwVector);
            currentRobotState = RobotState.detached;
            ResetArmPosition();
        }
        if (Input.GetMouseButtonDown(1)) {
            //Cancel the throwing, and go back to normal moving
            currentRobotState = RobotState.moving;
            trajectory.Hide();
            ResetArmPosition();
            ResetHeadPosition();
        }
    }

    IEnumerator Detached() {
        yield return new WaitForSeconds(0.5f);
        CheckForHead();


        if (Input.GetMouseButtonDown(0))
        {
            //Check if teleport power has recharged. Can't teleport unless it's charged.
            if (currentTeleportPower >= maxTeleportPower)
            {
                headTrans.parent = headParent;

                Teleport();

                currentRobotState = RobotState.teleporting;

                yield return new WaitForSeconds(0.2f);

                currentRobotState = RobotState.moving;
            }
        }
        // Reset back to body position. "Redo"
        else if (Input.GetMouseButtonDown(1)) {
            headTrans.parent = transform;
            Reset();
            currentRobotState = RobotState.teleporting;

            yield return new WaitForSeconds(0.2f);

            currentRobotState = RobotState.moving;
        }

        
    }


    void ThrowHead(Vector2 throwVector) {
        //Hide the trajectory efefct, and throw the head. Use the throw vector to throw the head in that direction. Remove it from the parent transform
        trajectory.Hide();
        headRB.bodyType = RigidbodyType2D.Dynamic;
        headRB.AddForce(throwVector, ForceMode2D.Impulse);
        headRB.AddTorque(Mathf.Clamp(throwVector.x,-2f,2f) * -throwVector.magnitude);
        headTrans.parent = null;

    }

    void Teleport() {


        //Deplete Teleport power to 0
        currentTeleportPower = 0;

        //Use half of the heads velocity
        rB.velocity = headRB.velocity * 0.75f;
        Vector2 headVel = headRB.velocity;

        // Play the particle system for teleporting. And delete it once it's finished playing.
        GameObject pS = Instantiate(teleportPS, headTrans.position, Quaternion.identity) as GameObject;
        Destroy(pS, pS.GetComponent<ParticleSystem>().main.duration);

        // Reset the head and put it back on the body once teleporting has finished

        headRB.bodyType = RigidbodyType2D.Kinematic;
        headRB.velocity = Vector2.zero;
        headRB.angularVelocity = 0;
        transform.position = headTrans.position;
        bodyTrans.localPosition = startBodyPos;
        headTrans.localPosition = startHeadPos;
        headTrans.rotation = Quaternion.Euler(Vector3.zero);
        
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

    void Reset() {
        // Reset the head back to the body. "Undo"
        headRB.bodyType = RigidbodyType2D.Kinematic;
        headRB.velocity = Vector2.zero;
        headRB.angularVelocity = 0;
        bodyTrans.localPosition = startBodyPos;
        headTrans.localPosition = startHeadPos;
        headTrans.localScale = bodyTrans.localScale;
        headTrans.rotation = Quaternion.Euler(Vector3.zero);


    }

    // Increasing the teleport power back to max. Over recharge rate time
    void RechargeTeleportPower() {
        if (currentTeleportPower < maxTeleportPower) {
            currentTeleportPower += teleportRechargeRate * Time.deltaTime;
        }

    }

    void SetThrowingArmAngle(Vector3 currentMousePos)
    {
        Vector3 diff = startMousePos - currentMousePos;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (diff.x > 0)
        {
            armSprite.localRotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (diff.x < 0)
        {
            armSprite.localRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void ResetArmPosition() {
        
        armSprite.localRotation = Quaternion.Euler(Vector3.zero);

    }
    void ResetHeadPosition() {
        headRB.bodyType = RigidbodyType2D.Kinematic;
        headRB.velocity = Vector2.zero;
        headRB.angularVelocity = 0;
        headTrans.parent = headParent;
        headTrans.localScale = bodyTrans.localScale;
        headTrans.localRotation = Quaternion.Euler(Vector3.zero);
        headTrans.localPosition = startHeadPos;
    }

    void CheckForHead() {
        if (Physics2D.OverlapCircle(transform.position, 1f, headLayer)) {
            ResetHeadPosition();
            currentRobotState = RobotState.moving;
        }

    }

    

    public void Die(DeathMethod theDeathMethod)
    {
        Debug.Log("Death by " + theDeathMethod);
        Vector3 newPos = (bodyTrans.position + headTrans.position) / 2;

        if (theDeathMethod == DeathMethod.electric)
        {
            if (currentRobotState != RobotState.detached)
            {
                Instantiate(electricDeath, newPos, Quaternion.identity);
                gameObject.SetActive(false);
            }
            else {
                Instantiate(electricDeath, headTrans.position, Quaternion.identity);
                headTrans.gameObject.SetActive(false);
                currentRobotState = RobotState.dead;

            }

        }
        StartCoroutine(ResetLevel());


        
        
    }

    IEnumerator ResetLevel() {
        yield return new WaitForSeconds(5);
        Debug.Log("Reset Level");


    }


}
