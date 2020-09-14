using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyButton : MonoBehaviour
{

    public GameObject buttonImage;
    public UnityEvent onButtonPush;
    public UnityEvent onButtonUp;
    public bool isPushed;
    public bool isMomentary;
    public LayerMask buttonLayer;
    public Vector3 buttonTriggerOffset;
    public Vector3 buttonTriggerRange;

    Vector3 _buttonStartPos;

    private void Start()
    {
        _buttonStartPos = buttonImage.transform.position;
    }

    private void Update()
    {
        ButtonCheck();
    }
    
    void ButtonCheck() {
        if (Physics2D.OverlapBox(transform.position + buttonTriggerOffset, buttonTriggerRange, 0f, buttonLayer))
        {
            if (!isPushed)
            {
                ButtonDown();
            }
        }
        else {
            if (isMomentary)
            {
                ButtonUp();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + buttonTriggerOffset, buttonTriggerRange);
    }
    
    void ButtonDown() {
        buttonImage.transform.localPosition = new Vector3(0, 0, 0);
        onButtonPush.Invoke();
        isPushed = true;
    }

    void ButtonUp() {
        buttonImage.transform.position = _buttonStartPos;
        onButtonUp.Invoke();
        isPushed = false;
    }

}
