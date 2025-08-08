using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerTag = "Player1"; // "Player1" ‚Ü‚½‚Í "Player2"
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 move;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // “ü—Í‚ðŽæ“¾
        if (playerTag == "Player1")
        {
            move.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
            move.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else if (playerTag == "Player2")
        {
            move.x = Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
            move.y = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = move.normalized * moveSpeed;
    }
}
