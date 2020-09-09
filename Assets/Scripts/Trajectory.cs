using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public int pixelNum;
    public Transform pixelParent;
    public GameObject trajectoryPixelPrefab;
    public GameObject trajectoryTargetPrefab;
    public float pixelSpacing;
    [Range(0,1)]
    public float minPixelSize = 1;
    [Range(0,1)]
    public float maxPixelSize = 1;

    public LayerMask collisionLayer;

    Transform[] pixelList;
    Vector2 pos;
    float timeStamp;
    float pixelSize;
    float sizeDiff;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        PreparePixels();
    }


    void PreparePixels() {
        pixelList = new Transform[pixelNum];
        sizeDiff = (maxPixelSize - minPixelSize) / pixelNum;
        pixelSize = maxPixelSize;
        for (int i = 0; i < pixelNum; i++)
        {
            pixelList[i] = Instantiate(trajectoryPixelPrefab, pixelParent).transform;
            pixelList[i].transform.localScale = Vector3.one * pixelSize;
            pixelSize -= sizeDiff;
            
        }

    }

    public void UpdateDots(Vector2 headPos, Vector2 forceDir) {
        timeStamp = pixelSpacing;

        for (int i = 0; i < pixelNum; i++)
        {
            pixelList[i].gameObject.SetActive(false);
            pixelList[i].GetComponent<SpriteRenderer>().color = Color.white;
        }

        for (int i = 0; i < pixelNum; i++)
        {
            pixelList[i].gameObject.SetActive(true);
            pos.x = (headPos.x + forceDir.x * timeStamp);
            pos.y = (headPos.y + forceDir.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            if (Physics2D.OverlapCircle(pos,0.1f,collisionLayer)) {
                pixelList[i].gameObject.SetActive(false);
                trajectoryTargetPrefab.SetActive(true);
                trajectoryTargetPrefab.transform.position = pos;

                return;
            }

            trajectoryTargetPrefab.SetActive(false);
            pixelList[i].position = pos;
            timeStamp += pixelSpacing;


        }


    }


    public void Show() {
        pixelParent.gameObject.SetActive(true);

    }

    public void Hide() {
        pixelParent.gameObject.SetActive(false);
        trajectoryTargetPrefab.SetActive(false);

    }
}
