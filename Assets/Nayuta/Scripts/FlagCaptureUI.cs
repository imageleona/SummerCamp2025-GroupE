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
        if (panel == null)
        {
            Debug.LogError("[FlagCaptureUI] panel is not assigned in Inspector.");
        }

        panel.SetActive(false);
    }

    void Update()
    {
        if (!panel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedFlagCount = Mathf.Max(minFlagCount, selectedFlagCount - 1);
            UpdateCounterText();
            Debug.Log("[FlagCaptureUI] Count decreased: " + selectedFlagCount);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedFlagCount = Mathf.Min(maxFlagCount, selectedFlagCount + 1);
            UpdateCounterText();
            Debug.Log("[FlagCaptureUI] Count increased: " + selectedFlagCount);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("[FlagCaptureUI] Confirm pressed. Selected: " + selectedFlagCount);
            onConfirm?.Invoke(selectedFlagCount);
            Close();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("[FlagCaptureUI] Cancel pressed.");
            onCancel?.Invoke();
            Close();
        }
    }

    public void Open(System.Action<int> confirmCallback, System.Action cancelCallback = null)
    {
        Debug.Log("[FlagCaptureUI] Open() called");

        onConfirm = confirmCallback;
        onCancel = cancelCallback;
        selectedFlagCount = 1;
        UpdateCounterText();

        if (panel == null)
        {
            Debug.LogError("[FlagCaptureUI] panel is NULL!");
        }
        else
        {
            Debug.Log("[FlagCaptureUI] panel.SetActive(true)");
            panel.SetActive(true);
        }
    }

    public void Close()
    {
        Debug.Log("[FlagCaptureUI] UI closed.");
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = "Flag Count: " + selectedFlagCount;
        }
        else
        {
            Debug.LogWarning("[FlagCaptureUI] counterText is NULL!");
        }
    }
}
