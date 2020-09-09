using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    public Transform floorPoint;
    public LayerMask floorMask;

    public LayerMask horizontalPlatformLayer;

    [Range(0f, 2f)]
    public float floorCheckRadius = 0.5f;


    private void Update()
    {

        CheckFloorRotation();
        CheckForHorizontalPlatform();
    }

    void CheckForHorizontalPlatform()
    {
        if (Physics2D.OverlapCircle(floorPoint.position, floorCheckRadius, horizontalPlatformLayer))
        {
            MovingWall wall = Physics2D.OverlapCircle(floorPoint.position, floorCheckRadius, horizontalPlatformLayer).gameObject.GetComponentInParent<MovingWall>();
            Transform parentPoint = wall.parentPoint;
            transform.parent = parentPoint;

            

        }
        else
        {
            transform.parent = null;

        }
    }


    void CheckFloorRotation() {
        if (Physics2D.OverlapCircle(floorPoint.position, floorCheckRadius, floorMask))
        {
            Transform floor = Physics2D.OverlapCircle(floorPoint.position, floorCheckRadius, floorMask).GetComponent<Transform>();
            transform.rotation = floor.rotation;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(floorPoint.position, floorCheckRadius);
    }



}
