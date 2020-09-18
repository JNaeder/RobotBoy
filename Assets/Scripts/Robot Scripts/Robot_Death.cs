using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Death : MonoBehaviour
{
    //--Robot Stuff--
    Robot_Main main;

    //--Other Stuff--
    GameManager gM;

    //--Death Prefabs
    public GameObject electricHead, electricBody;
    public GameObject squishedHead, squishedBody;

    //--Different Ways Of Dying--
    public enum DeathMethod { electric, squish, destroy };

    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Robot_Main>();

        gM = FindObjectOfType<GameManager>();
    }

    public void Die(DeathMethod theDeathMethod, GameObject theObject)
    {
        gM.PlayerDeath();

        main.currentRobotState = Robot_Main.RobotState.dead;
        if (theDeathMethod == DeathMethod.electric)
        {
            ElectricDeath(theObject);
        }
        else if (theDeathMethod == DeathMethod.squish)
        {
            SquishDeath(theObject);
        }
        else if (theDeathMethod == DeathMethod.destroy)
        {
            DestroyDeath(theObject);
        }


    }

    void ElectricDeath(GameObject _theObject) {
        if (_theObject == main.gameObject)
        {
            SpawnDeadObject(_theObject,electricBody, main.bodyTrans.position);
            SpawnDeadObject(_theObject,electricHead, main.headTrans.position);
        }
        else if (_theObject == main.headTrans.gameObject)
        {
            SpawnDeadObject(_theObject,electricHead, main.headTrans.position);
        }
    }

    void SquishDeath(GameObject _theObject) {
        if (_theObject == main.gameObject)
        {
            SpawnDeadObject(_theObject,squishedBody, main.bodyTrans.position);
            SpawnDeadObject(_theObject,squishedHead, main.bodyTrans.position);
        }
        else if (_theObject == main.headTrans.gameObject)
        {
            SpawnDeadObject(_theObject,squishedHead, main.headTrans.position);
        }
    }

    void DestroyDeath(GameObject _theObject) {
        _theObject.SetActive(false);

        //Maybe put a particle system or something here? 


    }

    void SpawnDeadObject(GameObject currentObject,GameObject deadObject, Vector2 spawnPos)
    {
        GameObject newObject = Instantiate(deadObject, spawnPos, Quaternion.identity) as GameObject;
        newObject.transform.localRotation = currentObject.transform.localRotation;

        currentObject.SetActive(false);
    }
}
