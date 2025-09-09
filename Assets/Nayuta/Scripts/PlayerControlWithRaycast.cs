using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlWithRaycast : MonoBehaviour
{
    public enum ControlScheme { Player1, Player2 }
    public ControlScheme controlScheme = ControlScheme.Player1;

    private Rigidbody rb;
    private float moveSpeed = 25f;

    private float lookSpeed = 100f;
    private float cameraPitch = 20f;

    public Transform cameraPivot;

    [Header("References")]
    [SerializeField] private FlagCaptureInput flagCaptureInput; // ← Inspectorからアサイン
    [SerializeField] private FlagCaptureUI flagCaptureUI;       // ← Inspectorからアサイン

    [Header("Raycast Settings")]
    public float rayDistance = 0.6f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
        HandleCamera();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // --- 入力処理 ---
    void HandleInput()
    {
        string prefix = controlScheme == ControlScheme.Player1 ? "_P1" : "_P2";

        // Capture開始 → Y
        if (Input.GetButtonDown("Capture" + prefix))
        {
            Debug.Log($"[{controlScheme}] Capture Start (Y)");
            flagCaptureInput?.StartCapture();
        }

        // Capture確定 → A
        if (Input.GetButtonDown("Confirm" + prefix))
        {
            Debug.Log($"[{controlScheme}] Capture Confirm (A)");
            flagCaptureInput?.ConfirmCapture();
        }

        // Cancel → B
        if (Input.GetButtonDown("Cancel" + prefix))
        {
            Debug.Log($"[{controlScheme}] Cancel (B)");
            flagCaptureInput?.CancelCapture();
        }

        // FlagDown → LB
        if (Input.GetButtonDown("FlagDown" + prefix))
        {
            Debug.Log($"[{controlScheme}] FlagDown (LB)");
            flagCaptureUI?.AdjustCount(-1);
        }

        // FlagUp → RB
        if (Input.GetButtonDown("FlagUp" + prefix))
        {
            Debug.Log($"[{controlScheme}] FlagUp (RB)");
            flagCaptureUI?.AdjustCount(1);
        }
    }

    // --- 移動処理 ---
    void HandleMovement()
    {
        string horizontal = controlScheme == ControlScheme.Player1 ? "Horizontal_P1" : "Horizontal_P2";
        string vertical = controlScheme == ControlScheme.Player1 ? "Vertical_P1" : "Vertical_P2";

        float h = Input.GetAxis(horizontal);
        float v = -Input.GetAxis(vertical); // 前後を反転

        Vector3 moveDirection = (transform.forward * v + transform.right * h).normalized;
        Vector3 targetPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // --- Raycastで壁チェック ---
        if (moveDirection != Vector3.zero)
        {
            if (Physics.Raycast(rb.position, moveDirection, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    Vector3 slide = Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;
                    targetPos = rb.position + slide * moveSpeed * Time.fixedDeltaTime;
                }
            }
        }

        rb.MovePosition(targetPos);
    }

    // --- カメラ処理 ---
    // カメラ周回用の距離
    public float cameraDistance = 7.5f;

    void HandleCamera()
    {
        string mouseX = controlScheme == ControlScheme.Player1 ? "RightStickX_P1" : "RightStickX_P2";
        string mouseY = controlScheme == ControlScheme.Player1 ? "RightStickY_P1" : "RightStickY_P2";

        float lookX = Input.GetAxis(mouseX);
        float lookY = -Input.GetAxis(mouseY);

        // --- 左右回転（プレイヤー本体を回す）
        if (Mathf.Abs(lookX) > 0.01f)
        {
            Quaternion delta = Quaternion.Euler(0f, lookX * lookSpeed * Time.deltaTime, 0f);
            rb.MoveRotation(rb.rotation * delta);
        }

        // --- 上下回転（カメラPivotをプレイヤーの周りに回す）
        cameraPitch = Mathf.Clamp(cameraPitch - lookY * lookSpeed * Time.deltaTime, -30f, 80f);

        if (cameraPivot != null)
        {
            Vector3 offset = new Vector3(0, 0, -cameraDistance);
            Quaternion rotation = Quaternion.Euler(cameraPitch, 0, 0);
            cameraPivot.localPosition = rotation * offset;
            cameraPivot.LookAt(transform.position + Vector3.up * 1.5f); // プレイヤーの頭あたりを見る
        }
    }

}
