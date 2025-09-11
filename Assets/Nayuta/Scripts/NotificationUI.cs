using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    public enum PlayerSide { Player1, Player2 }

    [Header("プレイヤー設定")]
    public PlayerSide playerSide = PlayerSide.Player1; // このUIがどちらのプレイヤー用か

    [Header("UI参照")]
    public GameObject leftPanel;      // 左から出すパネル
    public GameObject rightPanel;     // 右から出すパネル
    public Text leftMessageText;      // 左側のテキスト
    public Text rightMessageText;     // 右側のテキスト

    [Header("アニメーション設定")]
    public float displayTime = 3f;      // 表示時間（秒）
    public float slideDuration = 0.5f;  // スライド時間（秒）
    public float slideOffset = 300f;    // 画面外に隠れるオフセット距離

    private Coroutine currentRoutine;
    private RectTransform leftRect;
    private RectTransform rightRect;
    private Vector2 leftOriginalPos;
    private Vector2 rightOriginalPos;

    void Awake()
    {
        if (leftPanel != null)
        {
            leftRect = leftPanel.GetComponent<RectTransform>();
            leftOriginalPos = leftRect.anchoredPosition;
        }

        if (rightPanel != null)
        {
            rightRect = rightPanel.GetComponent<RectTransform>();
            rightOriginalPos = rightRect.anchoredPosition;
        }
    }

    void Start()
    {
        if (leftPanel != null) leftPanel.SetActive(false);
        if (rightPanel != null) rightPanel.SetActive(false);
    }

    /// <summary>
    /// 通知を表示する（このUIのプレイヤーに対応）
    /// </summary>
    public void Show(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        if (leftPanel == null || rightPanel == null) yield break;
        if (leftMessageText == null || rightMessageText == null) yield break;

        // メッセージをセット
        leftMessageText.text = message;
        rightMessageText.text = message;

        leftPanel.SetActive(true);
        rightPanel.SetActive(true);

        // 画面外からスタート位置を計算
        Vector2 leftStart = leftOriginalPos + Vector2.left * slideOffset;
        Vector2 rightStart = rightOriginalPos + Vector2.right * slideOffset;

        // スライドイン
        yield return StartCoroutine(Slide(leftRect, leftStart, leftOriginalPos, slideDuration));
        yield return StartCoroutine(Slide(rightRect, rightStart, rightOriginalPos, slideDuration));

        // 表示待機
        yield return new WaitForSeconds(displayTime);

        // スライドアウト
        yield return StartCoroutine(Slide(leftRect, leftOriginalPos, leftStart, slideDuration));
        yield return StartCoroutine(Slide(rightRect, rightOriginalPos, rightStart, slideDuration));

        leftPanel.SetActive(false);
        rightPanel.SetActive(false);

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
