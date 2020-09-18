using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    //--Robot Stuff--
    Robot_Main main;
    Robot_Death death;
    

    private void OnEnable()
    {
        //--Setup Robot Stuff
        main = FindObjectOfType<Robot_Main>();
        death = FindObjectOfType<Robot_Death>();
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

        //Check Object Destroyer
        if (collision.gameObject.tag == "Object Destroyer") {

            death.Die(Robot_Death.DeathMethod.destroy, gameObject);
        }
    }
}
