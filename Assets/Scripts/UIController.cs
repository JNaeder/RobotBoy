using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Image teleportRechargeImage;

    RobotMovement robotPlayer;
    // Start is called before the first frame update
    void Start()
    {
        robotPlayer = FindObjectOfType<RobotMovement>();


    }

    // Update is called once per frame
    void Update()
    {
        UpdateRechargeImage();
        

    }


    void UpdateRechargeImage() {
        float currentTeleportPower = robotPlayer.currentTeleportPower;
        float maxTeleportPower = robotPlayer.maxTeleportPower;
        float teleportPerc = currentTeleportPower / maxTeleportPower;


        Vector3 imageScale = teleportRechargeImage.rectTransform.localScale;
        imageScale.x = teleportPerc;
        teleportRechargeImage.rectTransform.localScale = imageScale;

    }

}
