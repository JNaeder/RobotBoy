using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConveyerBelt : MonoBehaviour
{
    [HideInInspector]public int conveyerBeltLength;
    [HideInInspector] public float beltSpeed;

    public GameObject[] beltPreFabs;



    public void BuildConveyerBelt(int theLength) {
        DeleteConveyerBelt();
        for (int i = 0; i < theLength; i++)
        {
            if (i == 0)
            {
                GameObject newBelt = Instantiate(beltPreFabs[0], transform.position + new Vector3(i, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
            }
            else if (i == theLength - 1)
            {
                GameObject newBelt = Instantiate(beltPreFabs[2], transform.position + new Vector3(i, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
            }
            else {
                GameObject newBelt = Instantiate(beltPreFabs[1], transform.position + new Vector3(i, 0, 0), Quaternion.identity, gameObject.transform) as GameObject;
            }
        }

        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        coll.size = new Vector2(conveyerBeltLength, 1);
        coll.offset = new Vector2((conveyerBeltLength / 2f) - 0.5f, 0);
    }



    public void DeleteConveyerBelt() {
        SpriteRenderer[] allObjects = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < allObjects.Length; i++)
        {
            DestroyImmediate(allObjects[i].gameObject);
        }
    }

    public void SetBeltSpeed(float _beltSpeed) {
        SurfaceEffector2D effector = GetComponent<SurfaceEffector2D>();
        effector.speed = _beltSpeed;


    }
}
