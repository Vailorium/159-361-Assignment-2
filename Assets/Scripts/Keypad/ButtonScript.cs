using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public int keypadNumber = 1;
    public UnityEvent KeypadClicked;

    private void OnMouseDown() 
    {
        Debug.Log("clicked!");
        KeypadClicked.Invoke();
    }
}
