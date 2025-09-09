using UnityEngine;

public class ResultDataManager : MonoBehaviour
{
    public static ResultDataManager Instance;

    // 各プレイヤーのエリア数と得点
    public int player1AreaCount;
    public int player2AreaCount;
    public int player1Score;
    public int player2Score;

    void Awake()
    {
        // Singletonパターン
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを跨いでも残す
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // データをセットする関数
    public void SetResult(int p1Areas, int p1Score, int p2Areas, int p2Score)
    {
        player1AreaCount = p1Areas;
        player1Score = p1Score;
        player2AreaCount = p2Areas;
        player2Score = p2Score;
    }
}
