using UnityEngine;

public class FlagCaptureInput : MonoBehaviour
{
    public PlayerTag playerTag;
    public PlayerFlagManager flagManager;
    public TerritoryManager territoryManager;
    public FlagCaptureUI captureUI;
    public WarningPopupUI warningUI;

    private TerritoryPlane currentPlane;

    // --- PlayerControlWithRaycastから呼ばれる ---
    public void StartCapture()
    {
        currentPlane = FindCurrentPlane();
        if (currentPlane != null)
        {
            captureUI.Open(OnConfirm, OnCancel);
            Debug.Log($"[CaptureInput] {playerTag} started flag selection for '{currentPlane.territoryName}'");
        }
        else
        {
            Debug.LogWarning($"[CaptureInput] {playerTag} could not find a TerritoryPlane to capture!");
            if (warningUI != null)
            {
                warningUI.Show("領域が見つかりません！");
            }
        }
    }

    public void ConfirmCapture()
    {
        captureUI.ForceConfirm(); // UIの確定処理を呼ぶ
    }

    public void CancelCapture()
    {
        captureUI.Close();
        Debug.Log($"[CaptureInput] {playerTag} canceled flag selection.");
    }

    // UIから呼ばれる
    void OnConfirm(int flagCount)
    {
        if (currentPlane == null) return;

        string name = currentPlane.territoryName;
        TerritoryOwner owner = (playerTag == PlayerTag.Player1) ? TerritoryOwner.Player1 : TerritoryOwner.Player2;

        if (flagManager.GetFlags(playerTag) < flagCount)
        {
            Debug.LogWarning($"[CaptureInput] Not enough flags. {playerTag} has {flagManager.GetFlags(playerTag)}");
            if (warningUI != null)
            {
                warningUI.Show("残りの旗の本数を超えているよ \n旗の本数を減らしてね");
            }
            return;
        }

        bool success = territoryManager.TryCapture(name, owner, flagCount);
        if (success)
        {
            flagManager.ConsumeFlags(playerTag, flagCount);
            Debug.Log($"[CaptureInput] {playerTag} captured '{name}' with {flagCount} flags.");
        }
        else
        {
            if (warningUI != null)
            {
                warningUI.Show("ここはすでに相手が獲得したエリアだよ \n取り返すには旗の本数を増やしてみよう！");
            }
            Debug.LogWarning($"[CaptureInput] Failed to capture '{name}'. Use more flags than previous owner.");
        }

        PrintStatus();
    }

    void OnCancel()
    {
        Debug.Log($"[CaptureInput] {playerTag} canceled flag selection.");
    }

    TerritoryPlane FindCurrentPlane()
    {
        TerritoryPlane[] planes = FindObjectsOfType<TerritoryPlane>();
        foreach (var plane in planes)
        {
            if (playerTag == PlayerTag.Player1 && plane.player1Inside)
                return plane;

            if (playerTag == PlayerTag.Player2 && plane.player2Inside)
                return plane;
        }

        return null;
    }

    void PrintStatus()
    {
        Debug.Log($"[Status] {playerTag} has {flagManager.GetFlags(playerTag)} flags remaining.");
        foreach (var kvp in territoryManager.GetAllTerritories())
        {
            Debug.Log($"[Status] {kvp.Key}: Owned by {kvp.Value.owner}, {kvp.Value.flagUsed} flags");
        }
    }
}
