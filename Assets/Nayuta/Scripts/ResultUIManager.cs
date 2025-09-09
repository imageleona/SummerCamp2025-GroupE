using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultUIManager : MonoBehaviour
{
    [Header("UI参照 - スコア表示用")]
    public Text player1AreaText;
    public Text player1ScoreText;
    public Text player2AreaText;
    public Text player2ScoreText;

    [Header("UI参照 - 勝敗表示用")]
    public GameObject resultUI;     // 通常のリザルト画面（エリア・得点表示）
    public GameObject player1WinUI; // Player1勝利用UI
    public GameObject player2WinUI; // Player2勝利用UI
    public GameObject drawUI;       // 引き分け用UI

    void Start()
    {
        if (ResultDataManager.Instance != null)
        {
            // --- Player1 ---
            if (player1AreaText != null)
                player1AreaText.text = ResultDataManager.Instance.player1AreaCount + " ";

            if (player1ScoreText != null)
                player1ScoreText.text = ResultDataManager.Instance.player1Score + " ";

            // --- Player2 ---
            if (player2AreaText != null)
                player2AreaText.text = ResultDataManager.Instance.player2AreaCount + " ";

            if (player2ScoreText != null)
                player2ScoreText.text = ResultDataManager.Instance.player2Score + " ";
        }
        else
        {
            Debug.LogError("[ScoreUIManager] ResultDataManager が存在しません！");
        }

        // 勝敗判定を10秒後に実行
        StartCoroutine(ShowWinnerAfterDelay(10f));
    }

    IEnumerator ShowWinnerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (ResultDataManager.Instance == null) yield break;

        int p1Score = ResultDataManager.Instance.player1Score;
        int p2Score = ResultDataManager.Instance.player2Score;

        // まずリザルトUIを非表示
        if (resultUI != null) resultUI.SetActive(false);

        // 勝敗判定
        if (p1Score > p2Score)
        {
            if (player1WinUI != null) player1WinUI.SetActive(true);
            Debug.Log("[Result] Player1 Win!");
        }
        else if (p2Score > p1Score)
        {
            if (player2WinUI != null) player2WinUI.SetActive(true);
            Debug.Log("[Result] Player2 Win!");
        }
        else
        {
            if (drawUI != null) drawUI.SetActive(true);
            Debug.Log("[Result] Draw!");
        }
    }
}
