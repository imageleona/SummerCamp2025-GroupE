using UnityEngine;

public class WASDMovements : MonoBehaviour
{
    public float moveSpeed = 3.0f; // 移動速度

    void Update()
    {
        // 入力を取得
        float horizontal = Input.GetAxis("Horizontal"); // A,Dキー → -1 ～ 1
        float vertical = Input.GetAxis("Vertical");     // W,Sキー → -1 ～ 1

        // 移動ベクトル
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        // カメラの向きに依存せず、ワールド座標で移動
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
}

