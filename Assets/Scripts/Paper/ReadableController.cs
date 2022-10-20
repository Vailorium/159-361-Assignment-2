using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadableController : MonoBehaviour, Interactable
{

    [SerializeField] Canvas childCanvas;
    [SerializeField] PlayerController pController;

    public void interact(PlayerController pC, GameObject obj = null)
    {
        childCanvas.gameObject.SetActive(true);
        pC.movementEnabled = false;
        pC.enableCursor();
    }

    public void close()
    {
        Debug.Log("Hello world!");
        childCanvas.gameObject.SetActive(false);
        pController.movementEnabled = true;
        pController.disableCursor();
    }
}
