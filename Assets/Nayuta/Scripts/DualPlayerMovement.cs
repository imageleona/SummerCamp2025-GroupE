using UnityEngine;

public class DualPlayerMovement : MonoBehaviour
{
    public enum ControlScheme { WASD, ArrowKeys }
    public ControlScheme controlScheme = ControlScheme.WASD;

    public float moveSpeed = 3.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 input = Vector3.zero;

        if (controlScheme == ControlScheme.WASD)
        {
            float h = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            float v = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            input = new Vector3(h, 0, v);
        }
        else if (controlScheme == ControlScheme.ArrowKeys)
        {
            float h = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            float v = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
            input = new Vector3(h, 0, v);
        }

        input = input.normalized; // ŽÎ‚ßˆÚ“®‚ð‘¬‚­‚µ‚È‚¢‚½‚ß
        Vector3 move = rb.position + input * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(move);
    }
}
