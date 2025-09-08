using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FlagUIManager : MonoBehaviour
{
    // === 1Pに表示するUI ===
    [Header("1P FlagUI")]
    [SerializeField] Image f_flagIcon;       // 旗のアイコン画像
    [SerializeField] Text f_flagCountText;   // 残り本数を表示するText（例：×30）
    [SerializeField] Text f_inputText;      // 「旗を何本使いますか？」を表示するテキスト
    [SerializeField] Text f_messageText; // 「相手プレイヤーが～」を表示するテキスト

    // === 1P内部で管理する値 ===
    private int f_currentFlags = 30;      // 現在残っている旗の本数
    private int f_selectCount = 0;   // 今回使う旗の本数（入力中の値）


    // === 2Pに表示するUI ===
    [Header("2P FlagUI")]
    [SerializeField] Image s_flagIcon;       // 旗のアイコン画像
    [SerializeField] Text s_flagCountText;   // 残り本数を表示するText（例：×30）
    [SerializeField] Text s_inputText;      // 「旗を何本使いますか？」を表示するテキスト
    [SerializeField] Text s_messageText; // 「相手プレイヤーが～」を表示するテキスト
    

    // === 2P内部で管理する値 ===
    private int s_currentFlags = 30;      // 現在残っている旗の本数
    private int s_selectCount = 0;   // 今回使う旗の本数（入力中の値）
  

    [Header("1P Animator")]
    [SerializeField] Animator f_anim;  // Inspectorで 1P用のAnimatorをドラッグ＆ドロップ

    [Header("2P Animator")]
    [SerializeField] Animator s_anim;  // Inspectorで 2P用のAnimatorをドラッグ＆ドロップ

    private GameObject f_parentPanel, s_parentPanel;


    void Start()
    {
        // UIを更新（×30 など）
        UpdateFlagUI();

        // メッセージは最初は非表示にしておく
        f_parentPanel = f_inputText.transform.parent.gameObject;
        s_parentPanel = s_inputText.transform.parent.gameObject;
        f_parentPanel.SetActive(false);
        s_parentPanel.SetActive(false);
        f_messageText.enabled = false;
        s_messageText.enabled = false;
    }

    void Update()
    {
        ///// ===== 1P操作 ===== /////
        // === Enterキー：入力開始 ===
        if (Input.GetKeyDown(KeyCode.Return))
        {
            f_parentPanel.SetActive(true);  // メッセージを表示
            f_inputText.text = "旗を何本使いますか？"; // 初期メッセージ
            f_selectCount = 0; // 入力中の数は0から
        }

        // === Qキー：旗数を増やす ===
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (f_selectCount < f_currentFlags)
            {
                f_selectCount++;  // 1本増やす
                f_inputText.text = $"旗を何本使いますか？\n{f_selectCount}本";
            }           
        }

        // === Eキー：旗数を減らす ===
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 0未満にはならないように制御
            f_selectCount = Mathf.Max(0, f_selectCount - 1);
            f_inputText.text = $"旗を何本使いますか？\n{f_selectCount}本";
        }

        // === Spaceキー：確定処理 ===
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 実際に旗を減らす
            UseFlags(f_selectCount, s_selectCount);

            // 入力用メッセージを非表示にする
            f_parentPanel.SetActive(false);

            // 1Pが立てた旗本数を表示する
            s_messageText.enabled = true;
            s_messageText.text = $"相手プレイヤーが〇〇エリアに {f_selectCount} 本の旗を立てました";
            s_anim.SetTrigger("2P_messageTrigger");  //アニメーションの実行
            StartCoroutine(HideAfterDelay(s_messageText, 5f));  //5秒後に非表示
            f_selectCount = 0;
        }



        //// ==== 2P操作 ==== ////
        // === Shiftキー：入力開始 ===
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            s_parentPanel.SetActive(true);  // メッセージを表示
            s_inputText.text = "旗を何本使いますか？"; // 初期メッセージ
            s_selectCount = 0; // 入力中の数は0から
        }

        // === Oキー：旗数を増やす ===
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (s_selectCount < s_currentFlags)
            {
                s_selectCount++;  // 1本増やす
                s_inputText.text = $"旗を何本使いますか？\n{s_selectCount}本";
            }
        }

        // === Pキー：旗数を減らす ===
        if (Input.GetKeyDown(KeyCode.P))
        {
            // 0未満にはならないように制御
            s_selectCount = Mathf.Max(0, s_selectCount - 1);
            s_inputText.text = $"旗を何本使いますか？\n{s_selectCount}本";
        }

        // === BackSpaceキー：確定処理 ===
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // 実際に旗を減らす
            UseFlags(f_selectCount, s_selectCount);

            // 入力用メッセージを非表示にする
            s_parentPanel.SetActive(false);

            // 2Pが立てた旗本数を表示する
            f_messageText.enabled = true;
            f_messageText.text = $"相手プレイヤーが〇〇エリアに {s_selectCount} 本の旗を立てました";
            f_anim.SetTrigger("1P_messageTrigger");  //アニメーションの実行
            StartCoroutine(HideAfterDelay(f_messageText, 5f));  //5秒後に非表示
            s_selectCount = 0;
        }
    }

    /// 指定された本数だけ旗を使用する処理
    void UseFlags(int f_count, int s_count)
    {
        //1P
        // 残り以上は使えないので制御
        int f_useCount = Mathf.Min(f_count, f_currentFlags);
        // 残り本数を減らす
        f_currentFlags -= f_useCount;


        //2P
        int s_useCount = Mathf.Min(s_count, s_currentFlags);
        // 残り本数を減らす
        s_currentFlags -= s_useCount;

        // 右上UIを更新（例：×27）
        UpdateFlagUI();
    }

    /// 右上の「旗アイコン × 残り本数」を更新する処理
    void UpdateFlagUI()
    {
        //1P
        if (f_flagCountText != null)
        {
            f_flagCountText.text = $"×{f_currentFlags}";
        }

        //2P
        if (s_flagCountText != null)
        {
            s_flagCountText.text = $"×{s_currentFlags}";
        }
    }

    /// 指定した Text を delay 秒後に非表示にする
    IEnumerator HideAfterDelay(Text target, float delay)
    {
        yield return new WaitForSeconds(delay);
        target.enabled = false;
    }
}
