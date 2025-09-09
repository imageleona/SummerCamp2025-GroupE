using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FlagCountUI : MonoBehaviour
{
    [Header("プレイヤー設定")]
    public PlayerTag playerTag; // Player1 or Player2
    public PlayerFlagManager flagManager; // 旗所持数を管理しているスクリプト

    [Header("UI参照")]
    public GameObject flagIconPrefab; // 旗アイコンのPrefab
    public Transform flagContainer;   // アイコンを並べる場所
    public Text flagText;             // 数字表示 (例: "25本")

    [Header("設定")]
    public int maxFlags = 30; // 最大旗数

    private List<GameObject> flagIcons = new List<GameObject>();

    void Start()
    {
        // 最大数分の旗アイコンを生成
        for (int i = 0; i < maxFlags; i++)
        {
            GameObject icon = Instantiate(flagIconPrefab, flagContainer);
            flagIcons.Add(icon);
        }

        UpdateFlagUI();
    }

    void Update()
    {
        UpdateFlagUI();
    }

    void UpdateFlagUI()
    {
        int currentFlags = flagManager.GetFlags(playerTag);

        // アイコンのON/OFF
        for (int i = 0; i < flagIcons.Count; i++)
        {
            flagIcons[i].SetActive(i < currentFlags);
        }

        // 数字表示を更新
        if (flagText != null)
        {
            flagText.text = currentFlags + " ";
        }
    }
}
