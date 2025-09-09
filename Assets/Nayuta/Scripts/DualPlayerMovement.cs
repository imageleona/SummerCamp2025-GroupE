using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class DualPlayerMovement : MonoBehaviour
{
    public enum ControlScheme { WASD, ArrowKeys }
    public ControlScheme controlScheme = ControlScheme.WASD;

    public float moveSpeed = 3.0f;
    private Rigidbody rb;

    [Header("Raycast設定")]
    public float rayDistance = 0.6f; // コライダー半径＋余裕

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        // 入力取得
        Vector3 input = GetInputDirection();
        if (input == Vector3.zero) return;

        // 進行方向にRaycastを飛ばす
        if (Physics.Raycast(rb.position, input, out RaycastHit hit, rayDistance))
        {
            if (!hit.collider.isTrigger && hit.collider.CompareTag("Wall"))
            {
                // 壁に当たった場合、スライド用に入力を壁の法線で投影
                Vector3 slideDirection = Vector3.ProjectOnPlane(input, hit.normal).normalized;
                input = slideDirection;
            }
        }

        // 移動
        Vector3 move = rb.position + input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(move);
    }

    private Vector3 GetInputDirection()
    {
        float h = 0, v = 0;

        if (controlScheme == ControlScheme.WASD)
        {
            h = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            v = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else if (controlScheme == ControlScheme.ArrowKeys)
        {
            h = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            v = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        }

        return new Vector3(h, 0, v).normalized;
    }
}
