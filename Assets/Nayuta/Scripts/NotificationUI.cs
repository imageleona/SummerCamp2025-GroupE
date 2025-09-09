using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    public GameObject panel;     // 通知表示用のパネル
    public Text messageText;     // 通知のテキスト
    public float displayTime = 3f; // 表示時間（秒）

    private Coroutine currentRoutine;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false); // 初期は非表示
    }

    /// <summary>
    /// 通知を表示する
    /// </summary>
    public void Show(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        if (panel != null && messageText != null)
        {
            messageText.text = message;
            panel.SetActive(true);

            yield return new WaitForSeconds(displayTime);

            panel.SetActive(false);
        }
        currentRoutine = null;
    }
}
