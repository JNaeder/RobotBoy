using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    //--Robot Stuff--
    Robot_Main main;
    Robot_Death death;



    // Start is called before the first frame update
    void Start()
    {
        //--Setup Robot Stuff
        main = GetComponentInParent<Robot_Main>();
        death = GetComponentInParent<Robot_Death>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //CheckSquisher
        if (collision.gameObject.tag == "TheSquisher")
        {
            death.Die(Robot_Death.DeathMethod.squish, gameObject);
        }

        //Check Electric Wall
        if (collision.gameObject.tag == "ElectricWalls")
        {
            death.Die(Robot_Death.DeathMethod.electric, gameObject);
        }


    }
}
