using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorObject;

    public void OpenDoor() {
        doorObject.SetActive(false);
    }

    public void CloseDoor() {
        doorObject.SetActive(true);
    }
}
