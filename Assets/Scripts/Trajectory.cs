using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public int pixelNum;
    public Transform pixelParent;
    public GameObject trajectoryPixelPrefab;
    public float pixelSpacing;

    Transform[] pixelList;
    Vector2 pos;
    float timeStamp;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        PreparePixels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PreparePixels() {
        pixelList = new Transform[pixelNum];
        for (int i = 0; i < pixelNum; i++)
        {
            pixelList[i] = Instantiate(trajectoryPixelPrefab, pixelParent).transform;
        }

    }

    public void UpdateDots(Vector2 headPos, Vector2 forceDir) {
        timeStamp = pixelSpacing;
        for (int i = 0; i < pixelNum; i++)
        {
            pos.x = (headPos.x + forceDir.x * timeStamp);
            pos.y = (headPos.y + forceDir.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            pixelList[i].position = pos;
            timeStamp += pixelSpacing;


        }


    }


    public void Show() {
        pixelParent.gameObject.SetActive(true);

    }

    public void Hide() {
        pixelParent.gameObject.SetActive(false);

    }
}
