using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarningPopupUI : MonoBehaviour
{
    public GameObject panel;      // パネル（背景）
    public Text warningText;      // 表示するテキスト
    public float displayTime = 3f; // 表示時間（秒）

    private float timer = 0f;
    private bool isShowing = false;

    void Update()
    {
        if (isShowing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                panel.SetActive(false);
                isShowing = false;
            }
        }
    }

    public void Show(string message)
    {
        warningText.text = message;
        panel.SetActive(true);
        timer = displayTime;
        isShowing = true;
    }
}
