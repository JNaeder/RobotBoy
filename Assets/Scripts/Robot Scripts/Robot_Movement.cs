using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Movement : MonoBehaviour
{
    public float movementSpeed;


    Robot_Main main;

    private void Start()
    {
        main = GetComponent<Robot_Main>();
    }

    public void Moving()
    {
        float h = Input.GetAxis("Horizontal");
        transform.position += new Vector3(h, 0, 0) * movementSpeed * Time.deltaTime;

        //update the animator wiht H
        main.anim.SetFloat("h", Mathf.Abs(h));

        //Turn the Player toward direction of movement
        if (h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


    }
}
