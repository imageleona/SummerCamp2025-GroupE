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
        panel.SetActive(false);
    }

    void Update()
    {
        if (!panel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedFlagCount = Mathf.Max(minFlagCount, selectedFlagCount - 1);
            UpdateCounterText();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedFlagCount = Mathf.Min(maxFlagCount, selectedFlagCount + 1);
            UpdateCounterText();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            onConfirm?.Invoke(selectedFlagCount);
            Close();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onCancel?.Invoke();
            Close();
        }
    }

    public void Open(System.Action<int> confirmCallback, System.Action cancelCallback = null)
    {
        onConfirm = confirmCallback;
        onCancel = cancelCallback;
        selectedFlagCount = 1;
        UpdateCounterText();
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    void UpdateCounterText()
    {
        counterText.text = $"Flag Count: {selectedFlagCount}";
    }
}
