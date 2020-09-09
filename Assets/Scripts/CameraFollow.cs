using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform targetBody, targetHead;
    public float moveSpeed;
    public Vector3 offset;
   
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 avgTarget = (targetBody.position + targetHead.position) / 2;
        Vector3 targetPos = new Vector3(avgTarget.x, avgTarget.y, -10);
        transform.position = Vector3.Lerp(transform.position, targetPos + offset, moveSpeed * Time.deltaTime);

    }
}
