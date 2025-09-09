using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TerritoryPlaneColorUpdater : MonoBehaviour
{
    [Header("参照するTerritoryManager")]
    public TerritoryManager territoryManager;

    [Header("このPlaneが対応するエリア名")]
    public string territoryName;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (territoryManager == null || string.IsNullOrEmpty(territoryName))
            return;

        // このエリアのオーナーを取得
        TerritoryOwner owner = territoryManager.GetOwner(territoryName);

        Color color = Color.white;

        if (owner == TerritoryOwner.Player1)
        {
            color = new Color(0f, 0f, 1f, 0.4f); // 半透明の青 (A=100/255　0.39 → 0.4f)
        }
        else if (owner == TerritoryOwner.Player2)
        {
            color = new Color(1f, 0f, 0f, 0.4f); // 半透明の赤
        }
        else
        {
            color = new Color(1f, 1f, 1f, 0f); // 未取得 → 完全透明
        }

        if (rend != null && rend.material != null)
        {
            rend.material.color = color;
        }
    }
}
