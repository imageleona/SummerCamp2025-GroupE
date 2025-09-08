using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Existing variables
    private Rigidbody rb;
    private Vector2 moveInput;
    private float moveSpeed = 5f;
    private float jumpForce = 5f;

    // --- Variables for looking ---
    private Vector2 lookInput;
    private float lookSpeed = 100f;
    private float cameraPitch = 20f; // Start with a slight downward angle

    // --- MODIFIED: Reference to the pivot and camera ---
    public Transform cameraPivot; // Assign this in the Inspector
    private Camera playerCamera;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>(); // Still need this for split-screen
    }

    void Start()
    {
        // Split-Screen Code (no changes)
        PlayerInput playerInput = GetComponent<PlayerInput>();

        if (playerInput.playerIndex == 0)
        {
            playerCamera.rect = new Rect(0, 0, 0.5f, 1);
        }
        else if (playerInput.playerIndex == 1)
        {
            playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }

    // OnMove and OnJump (no changes)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    // FixedUpdate for movement (no changes)
    void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }

    // --- MODIFIED: Update function for orbital camera rotation ---
    void Update()
    {
        // Horizontal rotation (left/right) still rotates the entire player body
        transform.Rotate(Vector3.up, lookInput.x * lookSpeed * Time.deltaTime);

        // Vertical input now controls the PITCH of the CameraPivot
        cameraPitch -= lookInput.y * lookSpeed * Time.deltaTime;
        // Clamp the pitch to create the orbital arc.
        // For example, 0 is horizontal, 80 is almost top-down.
        cameraPitch = Mathf.Clamp(cameraPitch, 0f, 80f);

        // Apply the pitch to the CameraPivot's local rotation
        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }
}