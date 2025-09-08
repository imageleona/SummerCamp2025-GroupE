using UnityEngine;

public class PlayerAreaUIController : MonoBehaviour
{
    [Header("どちらのプレイヤー用か")]
    public PlayerTag playerTag;

    [Header("TerritoryManager への参照")]
    public TerritoryManager territoryManager;

    [Header("UIの親オブジェクト (Canvas配下)")]
    public Transform uiParent; // ここに21枚のImageが入っている親を指定

    private GameObject currentActive;

    void Start()
    {
        // 初期化：全て非表示
        foreach (Transform child in uiParent)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (territoryManager == null || uiParent == null) return;

        // 今いるエリアを取得
        TerritoryPlane current = territoryManager.GetCurrentTerritory(playerTag);
        string areaName = current != null ? current.territoryName : "";

        // すでに表示しているのが同じなら何もしない
        if (currentActive != null && currentActive.name == areaName)
            return;

        // 一旦全部非表示
        foreach (Transform child in uiParent)
        {
            child.gameObject.SetActive(false);
        }

        // 今のエリア名と一致するオブジェクトだけ表示
        if (!string.IsNullOrEmpty(areaName))
        {
            Transform target = uiParent.Find(areaName);
            if (target != null)
            {
                target.gameObject.SetActive(true);
                currentActive = target.gameObject;
            }
            else
            {
                currentActive = null;
                Debug.LogWarning($"[PlayerAreaUIController] UIオブジェクトが見つかりません: {areaName}");
            }
        }
        else
        {
            currentActive = null;
        }
    }
}

