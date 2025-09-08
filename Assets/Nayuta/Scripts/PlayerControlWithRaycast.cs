using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerControlWithRaycast : MonoBehaviour
{
    // --- Movement variables ---
    private Rigidbody rb;
    private Vector2 moveInput;
    private float moveSpeed = 5f;
    private float jumpForce = 5f;

    // --- Camera control variables ---
    private Vector2 lookInput;
    private float lookSpeed = 100f;
    private float cameraPitch = 20f;

    public Transform cameraPivot; // Assign in Inspector
    private Camera playerCamera;

    [Header("Raycast Settings")]
    public float rayDistance = 0.6f; // Adjust to match collider radius

    // --- Game references ---
    private PlayerInput playerInput;
    private FlagCaptureInput flagCaptureInput;
    private FlagCaptureUI flagCaptureUI;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        flagCaptureInput = GetComponent<FlagCaptureInput>();
        flagCaptureUI = GetComponentInChildren<FlagCaptureUI>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        // Split-Screen
        if (playerInput.playerIndex == 0)
        {
            playerCamera.rect = new Rect(0, 0, 0.5f, 1);
        }
        else if (playerInput.playerIndex == 1)
        {
            playerCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }

    // --- Input callbacks ---
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

    // Capture開始 (Yボタン or F/J)
    public void OnCapture(InputAction.CallbackContext context)
    {
        if (context.performed && flagCaptureInput != null)
        {
            flagCaptureInput.StartCapture();
        }
    }

    // Capture確定 (Aボタン or Enter)
    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (context.performed && flagCaptureInput != null)
        {
            flagCaptureInput.ConfirmCapture();
        }
    }

    // Cancel (Escape or Bボタンなど割り当て自由)
    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed && flagCaptureInput != null)
        {
            flagCaptureInput.CancelCapture();
        }
    }

    // Flag数調整（LB/1 で減らす）
    public void OnFlagDown(InputAction.CallbackContext context)
    {
        if (context.performed && flagCaptureUI != null)
        {
            flagCaptureUI.AdjustCount(-1);
        }
    }

    // Flag数調整（RB/2 で増やす）
    public void OnFlagUp(InputAction.CallbackContext context)
    {
        if (context.performed && flagCaptureUI != null)
        {
            flagCaptureUI.AdjustCount(1);
        }
    }

    // --- Movement ---
    void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 desiredVelocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Raycast check for wall by tag
        Vector3 horizontalMove = new Vector3(moveDirection.x, 0, moveDirection.z);
        if (horizontalMove != Vector3.zero)
        {
            if (Physics.Raycast(rb.position, horizontalMove.normalized, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    // 壁にぶつかったらスライドする
                    Vector3 slide = Vector3.ProjectOnPlane(horizontalMove, hit.normal).normalized;
                    desiredVelocity = new Vector3(slide.x * moveSpeed, rb.velocity.y, slide.z * moveSpeed);
                }
            }
        }

        rb.velocity = desiredVelocity;
    }

    // --- Camera orbit ---
    void Update()
    {
        // Horizontal rotation → rotate whole body
        transform.Rotate(Vector3.up, lookInput.x * lookSpeed * Time.deltaTime);

        // Vertical rotation → only camera pivot
        cameraPitch -= lookInput.y * lookSpeed * Time.deltaTime;
        cameraPitch = Mathf.Clamp(cameraPitch, 0f, 80f);

        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
    }
}
