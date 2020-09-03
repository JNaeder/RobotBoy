using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{

    public enum MovingDirection {horizontal, vertical}
    public MovingDirection movingDirection;
    public GameObject wallObject;
    public float moveSpeed;
    public Transform endPointMax, endPointMin;
    public bool movingToMax;


    float sizeOfWall;

    // Start is called before the first frame update
    void Start()
    {
        if (movingDirection == MovingDirection.vertical)
        {
            sizeOfWall = wallObject.transform.localScale.y / 2;
        }

        if (movingDirection == MovingDirection.horizontal) {
            sizeOfWall = wallObject.transform.localScale.x / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (movingDirection == MovingDirection.vertical)
        {
            if (movingToMax)
            {
                MoveWallUp();
            }
            else
            {
                MoveWallDown();
            }
        }
        else if (movingDirection == MovingDirection.horizontal) {
            if (movingToMax)
            {
                MoveWallRight();
            }
            else {
                MoveWallLeft();
            }

        }



    }

    

    void MoveWallUp() {
        wallObject.transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (wallObject.transform.position.y >= endPointMax.position.y - sizeOfWall) {
            movingToMax = false;

        }


    }

    void MoveWallDown()
    {
        wallObject.transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
        if (wallObject.transform.position.y <= endPointMin.position.y + sizeOfWall)
        {
            movingToMax = true;

        }

    }

    void MoveWallRight() {
        wallObject.transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        if (wallObject.transform.position.x >= endPointMax.position.x - sizeOfWall)
        {
            movingToMax = false;

        }
    }

    void MoveWallLeft() {
        wallObject.transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
        if (wallObject.transform.position.x <= endPointMin.position.x + sizeOfWall)
        {
            movingToMax = true;

        }

    }
}
