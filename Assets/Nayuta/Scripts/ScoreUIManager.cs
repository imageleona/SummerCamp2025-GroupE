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

        // PlayerTag → TerritoryOwner 変換
        TerritoryOwner owner = (playerTag == PlayerTag.Player1) ? TerritoryOwner.Player1 : TerritoryOwner.Player2;

        // 指定されたプレイヤーのスコアを取得
        int score = territoryManager.GetScore(owner);

        // UIに表示（例: "30 point"）
        scoreText.text = $"{score}";
    }
}
