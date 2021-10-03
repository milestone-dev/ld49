using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Camera playerCamera;
    CharacterController characterController;
    public ParticleSystem warpParticles;

    public GameObject heldItemContainer;
    public GameObject heldItemAxe;
    public GameObject heldItemShovel;
    public GameObject heldItemVial;
    public GameObject heldItemFilledVial;
    public GameObject heldItemSodiumCrystal;
    public GameObject heldItemRuby;

    public float walkSpeed = 12;
    public float runSpeed = 24;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool frozen;

    // MouseLook
    public float mouseSensitivity = 500f;
    public float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        HideAllHeldItems();
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventoryManagement();
        UpdateCamera();
        UpdateMovement();
        UpdateInteraction();
    }

    public void MoveTo(Vector3 vec)
    {
        Debug.LogFormat("Moving to {0}", vec);
        transform.position = vec;
        velocity.y = 0;
        isGrounded = true;
        frozen = true;
        Invoke("Unfreeze", 0.01f);
        warpParticles.Play();
    }

    public void Unfreeze()
    {
        frozen = false;
    }

    void HideAllHeldItems()
    {
        heldItemAxe.SetActive(false);
        heldItemShovel.SetActive(false);
        heldItemVial.SetActive(false);
        heldItemFilledVial.SetActive(false);
        heldItemSodiumCrystal.SetActive(false);
        heldItemRuby.SetActive(false);
    }

    void UpdateInventoryManagement()
    {
        if (GameController.instance.PlayerIsHoldingItem(Item.None))
        {
            HideAllHeldItems();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            GameController.instance.SwitchToNextHeldItem();
            Debug.LogFormat("Switching to item: {0}", GameController.instance.heldItem);
            // TODO activate the right item
            switch(GameController.instance.heldItem)
            {
                case Item.Axe:
                    heldItemAxe.SetActive(true);
                    break;
                case Item.Shovel:
                    heldItemShovel.SetActive(true);
                    break;
                case Item.Vial:
                    heldItemVial.SetActive(true);
                    break;
                case Item.FilledVial:
                    heldItemFilledVial.SetActive(true);
                    break;
                case Item.SodiumCrystal:
                    heldItemSodiumCrystal.SetActive(true);
                    break;
                case Item.Ruby:
                    heldItemRuby.SetActive(true);
                    break;
            }
        }
    }

    void UpdateCamera()
    {
        if (GameController.instance.activeUIController != null)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void UpdateMovement()
    {
        if (frozen)
            return;

        if (GameController.instance.activeUIController != null)
            return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        characterController.Move(move * speed * Time.deltaTime);

        //Debug.LogFormat("{0} {1}", isGrounded, velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    void UpdateInteraction()
    {
        if (GameController.instance.activeUIController != null)
            return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool showCrosshair = false;
        if (Physics.Raycast(ray, out hit, 100))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable && interactable.interactionEnabled && interactable.triggerType == InteractionTriggerType.Click)
            {
                showCrosshair = true;
                GameController.instance.ActivateInteractionCursor();
                if (Input.GetMouseButtonDown(0))
                {
                    interactable.Interact();
                }
            }
        }
        if (!showCrosshair)
        {
            GameController.instance.DeactivateInteractionCursor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if (interactable && interactable.triggerType == InteractionTriggerType.Collide)
        {
            interactable.Interact();
        }
    }
}
