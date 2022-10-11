using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private float speed = 0.05f;

    public bool movementEnabled = true;

    [SerializeField] GameObject interactPanel;
    private bool interactPromptVisible = false;
    Interactable interactTarget = null;
    private float interactReach = 1f; // how close does the player have to be to interact with objects?

    [SerializeField] Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactTarget != null)
            {
                interactTarget.interact(gameObject);
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


            characterController.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 10, Space.World);

            Vector3 currentRot = characterController.transform.rotation.eulerAngles;

            mainCamera.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * 10, Space.World);
            mainCamera.transform.rotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, currentRot.y, 0);

            Vector3 direction = new Vector3(horizontal, 0, vertical);

            characterController.Move(transform.rotation * direction * speed);
        }
        FindInteractable();
    }

    void FindInteractable()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, transform.forward, out hit, interactReach))
        {
            Interactable interactableObject = hit.transform.GetComponent<Interactable>();

            if (interactableObject != null)
            {
                interactPanel.SetActive(true);
                interactPromptVisible = true;
                interactTarget = interactableObject;
            }
            else if (interactPromptVisible == true)
            {
                interactPanel.SetActive(false);
                interactPromptVisible = false;
            }
        } else if (interactPromptVisible == true)
        {
            interactPanel.SetActive(false);
            interactPromptVisible = false;
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
