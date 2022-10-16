using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaptopController : MonoBehaviour, Interactable
{
    GameObject consoleCanvas;
    [SerializeField] PlayerController pController;
    [SerializeField] Button closeButton;

    void Start()
    {
        consoleCanvas = this.transform.Find("TerminalCanvas").gameObject;

        closeButton.onClick.AddListener(close);
    }

    /// <summary>
    /// Open terminal panel
    /// </summary>
    public void interact(PlayerController pC, GameObject obj)
    {
        consoleCanvas.SetActive(true);
        pController.movementEnabled = false;
        pController.enableCursor();
    }

    /// <summary>
    /// Close terminal panel
    /// </summary>
    public void close()
    {
        consoleCanvas.SetActive(false);
        pController.movementEnabled = true;
        pController.disableCursor();
    }
}
