using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlWithRaycast : MonoBehaviour
{
    public enum ControlScheme { Player1, Player2 }
    public ControlScheme controlScheme = ControlScheme.Player1;

    private Rigidbody rb;
    private float moveSpeed = 50f;

    private float lookSpeed = 100f;
    private float cameraPitch = 20f;

    public Transform cameraPivot;

    [Header("References")]
    [SerializeField] private FlagCaptureInput flagCaptureInput;
    [SerializeField] private FlagCaptureUI flagCaptureUI;
    [SerializeField] private Animator animator;

    [Header("Raycast Settings")]
    public float rayDistance = 0.6f;

    private float currentSpeed = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        string[] joystickNames = Input.GetJoystickNames();
        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(joystickNames[i]))
            {
                Debug.Log($"Joystick {i + 1}: {joystickNames[i]}");
            }
        }
    }

    void Update()
    {
        HandleInput();
        HandleCamera();

        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
            //Debug.Log($"[{controlScheme}] Speed = {currentSpeed:F2}");
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        string prefix = controlScheme == ControlScheme.Player1 ? "_P1" : "_P2";

        if (Input.GetButtonDown("Capture" + prefix)) flagCaptureInput?.StartCapture();
        if (Input.GetButtonDown("Confirm" + prefix)) flagCaptureInput?.ConfirmCapture();
        if (Input.GetButtonDown("Cancel" + prefix)) flagCaptureInput?.CancelCapture();
        if (Input.GetButtonDown("FlagDown" + prefix)) flagCaptureUI?.AdjustCount(-1);
        if (Input.GetButtonDown("FlagUp" + prefix)) flagCaptureUI?.AdjustCount(1);
    }

    void HandleMovement()
    {
        string horizontal = controlScheme == ControlScheme.Player1 ? "Horizontal_P1" : "Horizontal_P2";
        string vertical = controlScheme == ControlScheme.Player1 ? "Vertical_P1" : "Vertical_P2";

        float h = Input.GetAxis(horizontal);
        float v = -Input.GetAxis(vertical);

        Vector3 moveDirection = (transform.forward * v + transform.right * h).normalized;
        Vector3 targetPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // --- 全方位Raycastチェック ---
        if (moveDirection != Vector3.zero)
        {
            Vector3[] directions = {
                transform.forward,
                -transform.forward,
                transform.right,
                -transform.right,
                (transform.forward + transform.right).normalized,
                (transform.forward - transform.right).normalized,
                (-transform.forward + transform.right).normalized,
                (-transform.forward - transform.right).normalized
            };

            foreach (var dir in directions)
            {
                if (Physics.Raycast(rb.position, dir, out RaycastHit hit, rayDistance))
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        // 壁が検出された → スライド
                        Debug.Log($"[{controlScheme}] Wall detected at {hit.point} in direction {dir}");
                        Vector3 slide = Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;
                        targetPos = rb.position + slide * moveSpeed * Time.fixedDeltaTime;
                        break; // 最初に当たった壁だけ処理
                    }
                }
            }
        }

        rb.MovePosition(targetPos);

        // アニメーション用
        currentSpeed = moveDirection.magnitude;
    }

    public float cameraDistance = 7.5f;

    void HandleCamera()
    {
        string mouseX = controlScheme == ControlScheme.Player1 ? "RightStickX_P1" : "RightStickX_P2";
        string mouseY = controlScheme == ControlScheme.Player1 ? "RightStickY_P1" : "RightStickY_P2";

        float lookX = Input.GetAxis(mouseX);
        float lookY = -Input.GetAxis(mouseY);

        if (Mathf.Abs(lookX) > 0.01f)
        {
            Quaternion delta = Quaternion.Euler(0f, lookX * lookSpeed * Time.deltaTime, 0f);
            rb.MoveRotation(rb.rotation * delta);
        }

        cameraPitch = Mathf.Clamp(cameraPitch - lookY * lookSpeed * Time.deltaTime, -30f, 80f);

        if (cameraPivot != null)
        {
            Vector3 offset = new Vector3(0, 0, -cameraDistance);
            Quaternion rotation = Quaternion.Euler(cameraPitch, 0, 0);
            cameraPivot.localPosition = rotation * offset;
            cameraPivot.LookAt(transform.position + Vector3.up * 1.5f);
        }
    }
}
