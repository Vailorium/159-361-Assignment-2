using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private float speed = 0.08f;

    public bool movementEnabled = true;

    [SerializeField] GameObject interactPanel;
    private bool interactPromptVisible = false;
    Interactable interactTarget = null;
    private float interactReach = 2f; // how close does the player have to be to interact with objects?

    [SerializeField] Camera mainCamera;

    public ChessPiece selectedPiece = null;

    private float yOrig;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        yOrig = transform.position.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactTarget != null)
            {
                interactTarget.interact(this, gameObject);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movementEnabled)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            Vector3 direction = new Vector3(horizontal, 0, vertical);

            characterController.Move(transform.rotation * direction * speed);
            transform.position.Set(transform.position.x, yOrig, transform.position.z);
            
            // force set character y-level (avoids collision bugs)
            if (transform.position.y > (yOrig*1.1)) {
                transform.position = new Vector3(transform.position.x, yOrig, transform.position.z);
            }
        }
        FindInteractable();
    }

    private void LateUpdate()
    {
        if (movementEnabled)
        {
            characterController.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 3, Space.World);

            mainCamera.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * 3, Space.Self);

            Vector3 rot = mainCamera.transform.localEulerAngles;

            // fix rotation to 50deg up, 60 deg down
            if(rot.x > 60 && rot.x < 270)
            {
                mainCamera.transform.localEulerAngles = new Vector3(60f, 0f, 0f);
            } else if(rot.x < 310 && rot.x > 90)
            {
                mainCamera.transform.localEulerAngles = new Vector3(-50f, 0f, 0f);
            }
          
        }
    }

    void FindInteractable()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // raycast from camera
        if(Physics.Raycast(ray, out hit, interactReach))
        {
            // find hit object
            Interactable interactableObject = hit.transform.GetComponent<Interactable>();

            // if an object has been hit, show panel
            if (interactableObject != null)
            {
                interactPanel.SetActive(true);
                interactPromptVisible = true;
                interactTarget = interactableObject;
            }
            else if (interactPromptVisible == true)
            {
                // otherwise hide panel
                interactPanel.SetActive(false);
                interactPromptVisible = false;
                interactTarget = null;
            }
        } else if (interactPromptVisible == true)
        {
            // hide panel if raycast didn't work
            interactPanel.SetActive(false);
            interactPromptVisible = false;
            interactTarget = null;
        }
    }

    public void disableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void enableCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
