using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WASDMovements : MonoBehaviour
{
    public float moveSpeed = 3.0f; // 移動速度
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody をキャッシュ
        rb = GetComponent<Rigidbody>();

        // Rigidbody の設定を推奨値に変更
        rb.isKinematic = false;           // 物理挙動を有効化
        rb.useGravity = true;             // 重力を有効化
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        // 回転は物理で勝手に傾かないよう固定
    }

    void FixedUpdate() // Rigidbody の移動は FixedUpdate でやる
    {
        // 入力を取得
        float horizontal = Input.GetAxis("Horizontal"); // A,Dキー → -1 ～ 1
        float vertical = Input.GetAxis("Vertical");     // W,Sキー → -1 ～ 1

        // 移動ベクトル
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // 移動先の座標を計算
        Vector3 targetPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

        // MovePosition で物理的に移動
        rb.MovePosition(targetPosition);
    }
}
