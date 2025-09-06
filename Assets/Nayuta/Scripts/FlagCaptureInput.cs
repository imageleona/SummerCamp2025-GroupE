using UnityEngine;

public class FlagCaptureInput : MonoBehaviour
{
    public PlayerTag playerTag;
    public PlayerFlagManager flagManager;
    public TerritoryManager territoryManager;
    public FlagCaptureUI captureUI;

    private TerritoryPlane currentPlane;

    void Update()
    {
        // 呼び出しキー：Player1 → F、Player2 → J
        if ((playerTag == PlayerTag.Player1 && Input.GetKeyDown(KeyCode.F)) ||
            (playerTag == PlayerTag.Player2 && Input.GetKeyDown(KeyCode.J)))
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
            }
        }
    }

    // UIで確定されたときに呼ばれる
    void OnConfirm(int flagCount)
    {
        if (currentPlane == null) return;

        string name = currentPlane.territoryName;
        TerritoryOwner owner = (playerTag == PlayerTag.Player1) ? TerritoryOwner.Player1 : TerritoryOwner.Player2;

        if (flagManager.GetFlags(playerTag) < flagCount)
        {
            Debug.LogWarning($"[CaptureInput] Not enough flags. {playerTag} has {flagManager.GetFlags(playerTag)}");
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
            //Debug.Log($"[FindPlane] Checking plane: {plane.territoryName}, P1 inside: {plane.player1Inside}, P2 inside: {plane.player2Inside}");

            if (playerTag == PlayerTag.Player1 && plane.player1Inside)
                return plane;

            if (playerTag == PlayerTag.Player2 && plane.player2Inside)
                return plane;
        }

        Debug.LogWarning($"[FindPlane] {playerTag} is not inside any TerritoryPlane.");
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
