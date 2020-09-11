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

    Vector3 _buttonStartPos;

    private void Start()
    {
        _buttonStartPos = buttonImage.transform.position;
    }

    private void Update()
    {
        ButtonCheck()

        if (isPushed) {
            ButtonDown();
        }
        else
        {
            ButtonUp();
        }
    }


    void ButtonCheck() {
        if (Physics2D.OverlapBox(transform.position, Vector2.one * 3f, 0f, buttonLayer)){
            Debug.Log("Hit Button");

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onButtonPush.Invoke();
        isPushed = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isMomentary && isPushed) {
            StartCoroutine(SetButtonFalse());
        }
    }

    IEnumerator SetButtonFalse() {
        yield return new WaitForSeconds(0.3f);
        isPushed = false;
        onButtonUp.Invoke();
    }

    void ButtonDown() {
        buttonImage.transform.localPosition = new Vector3(0, 0, 0);
    }

    void ButtonUp() {
        buttonImage.transform.position = _buttonStartPos;
    }

}
