using UnityEngine;
using UnityEngine.UI;

public class FlagCaptureUI : MonoBehaviour
{
    public GameObject panel;
    public Text counterText;
    public int selectedFlagCount = 1;
    public int minFlagCount = 1;
    public int maxFlagCount = 99;

    private System.Action<int> onConfirm;
    private System.Action onCancel;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // PlayerControlWithRaycast ‚©‚çŒÄ‚Î‚ê‚é
    public void AdjustCount(int delta)
    {
        if (!panel.activeSelf) return;
        selectedFlagCount = Mathf.Clamp(selectedFlagCount + delta, minFlagCount, maxFlagCount);
        UpdateCounterText();
    }

    public void ForceConfirm()
    {
        if (!panel.activeSelf) return;
        onConfirm?.Invoke(selectedFlagCount);
        Close();
    }

    public void ForceCancel()
    {
        if (!panel.activeSelf) return;
        onCancel?.Invoke();
        Close();
    }

    public void Open(System.Action<int> confirmCallback, System.Action cancelCallback = null)
    {
        onConfirm = confirmCallback;
        onCancel = cancelCallback;
        selectedFlagCount = 1;
        UpdateCounterText();
        if (panel != null) panel.SetActive(true);
    }

    public void Close()
    {
        if (panel != null) panel.SetActive(false);
    }

    void UpdateCounterText()
    {
        if (counterText != null)
            counterText.text = selectedFlagCount + "–{";
    }
}
