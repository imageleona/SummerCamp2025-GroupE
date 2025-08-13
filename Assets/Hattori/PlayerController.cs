using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum ControlType { WASD, Arrow }
    public ControlType controlType = ControlType.WASD;  // デフォルトはWASD

    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (controlType == ControlType.WASD)
        {
            input.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
            input.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else if (controlType == ControlType.Arrow)
        {
            input.x = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            input.y = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = input.normalized * moveSpeed;
    }
}
