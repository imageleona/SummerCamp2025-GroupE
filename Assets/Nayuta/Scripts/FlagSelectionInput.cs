using UnityEngine;

public class FlagSelectionInput : MonoBehaviour
{
    public PlayerTag playerTag;
    public PlayerFlagManager flagManager;
    public TerritoryManager territoryManager;

    private int selectedFlagCount = 1;
    private bool isSelecting = false;
    private TerritoryPlane currentPlane;

    void Update()
    {
        // エリア取得呼び出し
        if (!isSelecting)
        {
            if ((playerTag == PlayerTag.Player1 && Input.GetKeyDown(KeyCode.F)) ||
                (playerTag == PlayerTag.Player2 && Input.GetKeyDown(KeyCode.J)))
            {
                // 今いるTerritoryPlaneを探す
                currentPlane = FindCurrentPlane();
                if (currentPlane != null)
                {
                    isSelecting = true;
                    selectedFlagCount = 1;
                    Debug.Log($"[FlagSelection] {playerTag} started selecting flags for {currentPlane.territoryName}");
                }
            }
        }
        else
        {
            // カウント調整
            if (Input.GetKeyDown(KeyCode.Alpha1)) selectedFlagCount = Mathf.Max(1, selectedFlagCount - 1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) selectedFlagCount += 1;

            Debug.Log($"[FlagSelection] {playerTag} selected flag count: {selectedFlagCount}");

            // 確定
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentPlane != null)
                {
                    string name = currentPlane.territoryName;
                    TerritoryOwner owner = (playerTag == PlayerTag.Player1) ? TerritoryOwner.Player1 : TerritoryOwner.Player2;

                    // フラッグ足りる？
                    if (flagManager.GetFlags(playerTag) >= selectedFlagCount)
                    {
                        bool success = territoryManager.TryCapture(name, owner, selectedFlagCount);
                        if (success)
                        {
                            flagManager.ConsumeFlags(playerTag, selectedFlagCount);
                            Debug.Log($"[FlagSelection] {playerTag} captured {name} with {selectedFlagCount} flags.");
                        }
                        else
                        {
                            Debug.Log($"[FlagSelection] {name} capture failed. Not enough flags or already owned.");
                        }
                    }
                    else
                    {
                        Debug.Log($"[FlagSelection] Not enough flags to capture {name}.");
                    }

                    // フィードバック
                    Debug.Log($"[Status] {playerTag} now has {flagManager.GetFlags(playerTag)} flags.");
                    foreach (var kvp in territoryManager.GetAllTerritories())
                    {
                        Debug.Log($"[Status] {kvp.Key}: {kvp.Value.owner}, {kvp.Value.flagUsed} flags used");
                    }
                }

                isSelecting = false;
            }
        }
    }

    // 自分が今乗っているTerritoryPlaneを見つける
    TerritoryPlane FindCurrentPlane()
    {
        foreach (var plane in FindObjectsOfType<TerritoryPlane>())
        {
            if (playerTag == PlayerTag.Player1 && plane.player1Inside) return plane;
            if (playerTag == PlayerTag.Player2 && plane.player2Inside) return plane;
        }
        return null;
    }
}
