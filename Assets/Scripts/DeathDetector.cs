using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    RobotMovement robotPlayer;


    // Start is called before the first frame update
    void Start()
    {
        robotPlayer = GetComponent<RobotMovement>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //CheckSquisher
        if (collision.gameObject.tag == "TheSquisher")
        {
            robotPlayer.Die(RobotMovement.DeathMethod.squish);
        }

        //Check Electric Wall
        if (collision.gameObject.tag == "ElectricWalls")
        {
            robotPlayer.Die(RobotMovement.DeathMethod.electric);
        }


    }
}
