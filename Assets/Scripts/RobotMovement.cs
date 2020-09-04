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
    public Transform bodyTrans, headTrans;
    // The rigidbody of the head
    public Rigidbody2D headRB;
    // Min and Max Trajectory Magnitudes
    public float trajectoryMin, trajectoryMax;

    // Different States Of the Throwing Mechanic
    public enum RobotState {moving, throwing, detached, teleporting}
    public RobotState currentRobotState = RobotState.moving;

    //Init positions of mouse and player
    Vector3 startMousePos, currentMousePos;
    Vector3 startBodyPos, startHeadPos;
    Vector2 throwVector;

    // refrences to camera and player Rigidbody
    Camera cam;
    Rigidbody2D rB;

    //effect prefabs
    public Trajectory trajectory;
    public GameObject teleportPS;

    private void Start()
    {
        //Set up all these things
        cam = Camera.main;
        rB = GetComponent<Rigidbody2D>();
        startBodyPos = bodyTrans.localPosition;
        startHeadPos = headTrans.localPosition;

    }

    private void Update()
    {

        // Control the states of the Robot
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

    //Moving Left And Right
    void Movement() {
        float h = Input.GetAxis("Horizontal");
        transform.position += new Vector3(h, 0, 0) * movementSpeed * Time.deltaTime;

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
        }
    }

    IEnumerator Detached() {
        if (Input.GetMouseButtonDown(0))
        {
            headTrans.parent = transform;

            Teleport();

            currentRobotState = RobotState.teleporting;

            yield return new WaitForSeconds(0.2f);

            currentRobotState = RobotState.moving;
        }
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
        headTrans.parent = null;

    }

    void Teleport() {
        //Use half of the heads velocity
        rB.velocity = headRB.velocity / 2;

        // Play the particle system for teleporting. And delete it once it's finished playing.
        GameObject pS = Instantiate(teleportPS,headTrans.position, Quaternion.identity) as GameObject;
        Destroy(pS, pS.GetComponent<ParticleSystem>().main.duration);

        // Reset the head and put it back on the body once teleporting has finished
        headRB.bodyType = RigidbodyType2D.Kinematic;
        headRB.velocity = Vector2.zero;
        transform.position = headTrans.position;
        bodyTrans.localPosition = startBodyPos;
        headTrans.localPosition = startHeadPos;
        
        

    }

    void Reset() {
        // Reset the head back to the body. "Undo"
        headRB.bodyType = RigidbodyType2D.Kinematic;
        headRB.velocity = Vector2.zero;
        bodyTrans.localPosition = startBodyPos;
        headTrans.localPosition = startHeadPos;


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Set it so player follows horizontal platforms. By Setting the parent
        if (collision.gameObject.tag == "Horizontal Platform") {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // setting the parent back to null after player leaves platform
        if (collision.gameObject.tag == "Horizontal Platform") {
            transform.parent = null;

        }
    }


}
