using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyButton : MonoBehaviour
{

    public GameObject buttonImage;
    public UnityEvent onButtonPush;
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        onButtonPush.Invoke();
        PushButtonDown();
    }

    void PushButtonDown() {
        buttonImage.transform.localPosition = new Vector3(0, 0, 0);

    }


}
