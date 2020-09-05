using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWalls : MonoBehaviour
{
    RobotMovement robotPlayer;

    // Start is called before the first frame update
    void Start()
    {
        robotPlayer = FindObjectOfType<RobotMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            robotPlayer.Die();

        }
    }
}
