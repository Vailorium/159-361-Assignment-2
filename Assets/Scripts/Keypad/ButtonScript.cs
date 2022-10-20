using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour, Interactable
{
    public int keypadNumber = 1;
    public UnityEvent KeypadClicked;

    public void interact(PlayerController pC = null, GameObject obj = null)
    {
        KeypadClicked.Invoke();
    }

    private void OnMouseDown() 
    {
        KeypadClicked.Invoke();
    }
}
