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
    public Transform parentPoint;

    [HideInInspector]
    public float sizeOfWall;

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
                MoveWall(Vector3.up, endPointMax, MovingDirection.vertical);
            }
            else
            {
                MoveWall(Vector3.down, endPointMin, MovingDirection.vertical);
            }
        }
        else if (movingDirection == MovingDirection.horizontal) {
            if (movingToMax)
            {
                MoveWall(Vector3.right, endPointMax, MovingDirection.horizontal);
            }
            else {
                MoveWall(Vector3.left, endPointMin, MovingDirection.horizontal);
            }

        }



    }

    void MoveWall(Vector3 direction, Transform endPoint, MovingDirection moveDir) {
        wallObject.transform.position += direction * moveSpeed * Time.deltaTime;
        parentPoint.transform.position += direction * moveSpeed * Time.deltaTime;

        if (moveDir == MovingDirection.horizontal) {
            if (movingToMax)
            {
                if (wallObject.transform.position.x >= endPoint.position.x - sizeOfWall)
                {
                    movingToMax = !movingToMax;
                }
            }
            else {
                if (wallObject.transform.position.x <= endPoint.position.x + sizeOfWall)
                {
                    movingToMax = !movingToMax;
                }
            }

        } else if(moveDir == MovingDirection.vertical)
        {
            if (movingToMax)
            {
                if (wallObject.transform.position.y >= endPoint.position.y - sizeOfWall)
                {
                    movingToMax = !movingToMax;
                }
            }
            else {
                if (wallObject.transform.position.y <= endPoint.position.y + sizeOfWall)
                {
                    movingToMax = !movingToMax;
                }
            }

        }




    }

    

    void MoveWallUp() {
        wallObject.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        if (movingDirection == MovingDirection.horizontal)
        {
            parentPoint.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }

        if (wallObject.transform.position.y >= endPointMax.position.y - sizeOfWall) {
            movingToMax = false;

        }


    }
}
