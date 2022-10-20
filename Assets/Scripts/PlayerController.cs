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
        }
        FindInteractable();
    }

    private void LateUpdate()
    {
        if (movementEnabled)
        {
            characterController.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 3, Space.World);

            Vector3 currentRot = characterController.transform.rotation.eulerAngles;

            mainCamera.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * 3, Space.Self);
        }
    }

    void FindInteractable()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, interactReach))
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
                interactTarget = null;
            }
        } else if (interactPromptVisible == true)
        {
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
