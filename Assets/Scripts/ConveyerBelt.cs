using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Transform playerTrans = collision.gameObject.GetComponent<Transform>();
            playerTrans.rotation = transform.rotation;
            playerTrans.position += new Vector3(0, -0.3f, 0);

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Transform playerTrans = collision.gameObject.GetComponent<Transform>();
            Debug.Log("Exit");
            playerTrans.rotation = Quaternion.Euler(Vector3.zero);


        }
    }

}
