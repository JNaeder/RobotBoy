using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //--Other Scripts--
    SceneLoader sceneLoader;
    UIController uIController;

    // Start is called before the first frame update
    void Start()
    {
        //Set Up Refrences
        sceneLoader = GetComponent<SceneLoader>();
        uIController = FindObjectOfType<UIController>();
    }

    public void PlayerDeath() {
        uIController.ShowDeathScreen();
    }

    public void RestartLevel() {
        int thisBuildIndex = SceneManager.GetActiveScene().buildIndex;
        sceneLoader.LoadScene(thisBuildIndex);

    }




}
