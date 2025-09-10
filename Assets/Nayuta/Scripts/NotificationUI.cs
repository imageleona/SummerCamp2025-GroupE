using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    public enum PlayerSide { Player1, Player2 }
    public enum SlideDirection { Left, Right }

    [Header("プレイヤー設定")]
    public PlayerSide playerSide = PlayerSide.Player1; // このUIがどちらのプレイヤー用か
    public SlideDirection slideDirection = SlideDirection.Left; // スライド方向

    [Header("UI参照")]
    public GameObject panel;     // 通知表示用のパネル
    public Text messageText;     // 通知のテキスト

    [Header("アニメーション設定")]
    public float displayTime = 3f;   // 表示時間（秒）
    public float slideDuration = 0.5f; // スライド時間（秒）
    public float slideOffset = 300f;   // 画面外に隠れるオフセット距離

    private Coroutine currentRoutine;
    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Awake()
    {
        if (panel != null)
            rectTransform = panel.GetComponent<RectTransform>();

        if (rectTransform != null)
            originalPosition = rectTransform.anchoredPosition;
    }

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
        if (panel == null || messageText == null || rectTransform == null)
            yield break;

        messageText.text = message;
        panel.SetActive(true);

        // 初期位置（画面外）
        Vector2 startPos = originalPosition;
        if (slideDirection == SlideDirection.Left)
            startPos.x -= slideOffset;
        else
            startPos.x += slideOffset;

        // 画面内位置（元の位置）
        Vector2 endPos = originalPosition;

        // スライドイン
        yield return StartCoroutine(Slide(rectTransform, startPos, endPos, slideDuration));

        // 一定時間表示
        yield return new WaitForSeconds(displayTime);

        // スライドアウト
        yield return StartCoroutine(Slide(rectTransform, endPos, startPos, slideDuration));

        panel.SetActive(false);
        currentRoutine = null;
    }

    private IEnumerator Slide(RectTransform rect, Vector2 from, Vector2 to, float duration)
    {
        float time = 0f;
        rect.anchoredPosition = from;

        while (time < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = to;
    }
}
