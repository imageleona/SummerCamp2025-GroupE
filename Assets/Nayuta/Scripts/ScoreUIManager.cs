using UnityEngine;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour
{
    [Header("TerritoryManager 参照")]
    public TerritoryManager territoryManager;

    [Header("このUIが表示するプレイヤー")]
    public PlayerTag playerTag;

    [Header("スコア表示 UI Text")]
    public Text scoreText;

    void Update()
    {
        if (territoryManager == null || scoreText == null) return;

        // 指定されたプレイヤーのスコアを取得
        int score = territoryManager.GetScore(playerTag);

        // UIに表示
        scoreText.text = $"{score}";
    }
}
