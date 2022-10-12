using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadableController : MonoBehaviour, Interactable
{

    [SerializeField] Canvas childCanvas;
    [SerializeField] PlayerController pController;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void interact(GameObject obj = null)
    {
        childCanvas.gameObject.SetActive(true);
        pController.movementEnabled = false;
        pController.enableCursor();
    }

    public void close()
    {
        childCanvas.gameObject.SetActive(false);
        pController.movementEnabled = true;
        pController.disableCursor();
    }
}
