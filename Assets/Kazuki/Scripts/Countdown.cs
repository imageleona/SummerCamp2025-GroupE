using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text timerText;       // UIのTextをInspectorで設定
    public int currentTime = 10; // 制限時間（秒）
    private float timer = 1f;    // 1秒ごとのカウント用

    void Update()
    {
        timer -= Time.deltaTime; // 経過時間を減らす

        if (timer <= 0f && currentTime > 0)
        {
            currentTime--; // 1秒経過ごとに-1
            timer = 1f;    // カウントリセット
            timerText.text = "残り" + currentTime.ToString() + "秒"; // UI更新

            if (currentTime <= 0)
            {
                timerText.text = "Time Up!";
                Debug.Log("ゲーム終了");
            }
        }
    }
}
