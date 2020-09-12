using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelChangeDoor : MonoBehaviour
{
    public UnityEvent onEnterDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            onEnterDoor.Invoke();
        }
    }


}
