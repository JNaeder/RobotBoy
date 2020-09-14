using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject deathScreen;
    

    public void ShowDeathScreen() {
        deathScreen.SetActive(true);
    }

}
