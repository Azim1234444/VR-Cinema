using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;
    public float superSprintSpeed = 16f;

    [Header("Jump & Gravity")]
    public float jumpPower = 7f;
    public float gravity = 10f;
    [Tooltip("How hard we press down when grounded to 'stick' to slopes/stairs")]
    public float stickToGroundForce = 5f;

    [Header("Crouch")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    [Header("Double-tap & Timers")]
    public float doubleTapTime = 0.3f;   // to detect W double-tap
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    // state
    private bool canMove = true;
    private bool isCrouching = false;
    private float lastTapTimeW = -1f;
    private bool doubleTappedW = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // —— IMPORTANT FOR STAIRS —— 
        // Make sure these values are high enough to step up your stair height
        characterController.stepOffset = 1f;
        characterController.slopeLimit = 60f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ——— Mouse Look ———
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        // ——— Detect W double-tap ———
        if (Input.GetKeyDown(KeyCode.W))
        {
            doubleTappedW = Time.time - lastTapTimeW < doubleTapTime;
            lastTapTimeW = Time.time;
        }

        // ——— Choose Speed ———
        float currentSpeed = walkSpeed;
        bool holdingShift = Input.GetKey(KeyCode.LeftShift);
        if (doubleTappedW && holdingShift) currentSpeed = superSprintSpeed;
        else if (doubleTappedW || holdingShift) currentSpeed = sprintSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;

        // ——— Build planar movement vector ———
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float vx = canMove ? currentSpeed * Input.GetAxis("Vertical") : 0;
        float vz = canMove ? currentSpeed * Input.GetAxis("Horizontal") : 0;

        // preserve existing Y momentum
        float y = moveDirection.y;
        moveDirection = forward * vx + right * vz;
        moveDirection.y = y;

        // ——— Jump or Stick to Ground ———
        if (characterController.isGrounded)
        {
            // If we press jump—and not crouching—we launch up
            if (Input.GetButton("Jump") && !isCrouching && canMove)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                // Otherwise press us down onto the ground so we can climb stairs
                moveDirection.y = -stickToGroundForce;
            }
        }
        else
        {
            // mid-air gravity
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // ——— Crouch ———
        if (Input.GetKey(KeyCode.Z))
        {
            characterController.height = crouchHeight;
            isCrouching = true;
        }
        else
        {
            characterController.height = defaultHeight;
            isCrouching = false;
        }

        // ——— Finally, move! ———
        characterController.Move(moveDirection * Time.deltaTime);
    }
}