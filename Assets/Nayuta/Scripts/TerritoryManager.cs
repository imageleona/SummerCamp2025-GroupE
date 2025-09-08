using System.Collections.Generic;
using UnityEngine;

public enum TerritoryOwner
{
    None,
    Player1,
    Player2
}

public class TerritoryManager : MonoBehaviour
{
    private Dictionary<string, (TerritoryOwner owner, int flagUsed)> territories = new();
    private Dictionary<string, int> territoryPoints = new(); // エリアごとの点数

    [Header("エリア一覧 (シーンにあるTerritoryPlane)")]
    public TerritoryPlane[] territoryPlanes;

    void Start()
    {
        AssignRandomPoints();
    }

    /// <summary>
    /// エリアに点数をランダムに割り当てる
    /// </summary>
    void AssignRandomPoints()
    {
        List<int> pointsPool = new List<int>();

        // 点数配分を作成
        pointsPool.AddRange(CreateList(5, 8));
        pointsPool.AddRange(CreateList(10, 7));
        pointsPool.AddRange(CreateList(15, 3));
        pointsPool.AddRange(CreateList(20, 2));
        pointsPool.AddRange(CreateList(50, 1));

        // シャッフル
        for (int i = 0; i < pointsPool.Count; i++)
        {
            int rand = Random.Range(i, pointsPool.Count);
            (pointsPool[i], pointsPool[rand]) = (pointsPool[rand], pointsPool[i]);
        }

        // 各エリアに割り当て
        for (int i = 0; i < territoryPlanes.Length && i < pointsPool.Count; i++)
        {
            string name = territoryPlanes[i].territoryName;
            territoryPoints[name] = pointsPool[i];
            Debug.Log($"[TerritoryManager] {name} に {pointsPool[i]} 点を割り当てました");
        }
    }

    private List<int> CreateList(int value, int count)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++) list.Add(value);
        return list;
    }

    // 既存のTryCapture
    public bool TryCapture(string name, TerritoryOwner player, int flagCount)
    {
        if (!territories.ContainsKey(name))
        {
            territories[name] = (TerritoryOwner.None, 0);
        }

        var data = territories[name];

        if (data.owner == player)
        {
            return false; // 自分の陣地なら無視
        }

        if (data.owner == TerritoryOwner.None || flagCount > data.flagUsed)
        {
            // 所有者と使用本数を更新
            territories[name] = (player, flagCount);

            // 色変更を反映
            TerritoryPlane plane = FindPlaneByName(name);
            if (plane != null)
            {
                plane.SetOwner(player);
            }

            return true;
        }

        return false;
    }

    // 既存のRelease
    public void Release(string name, TerritoryOwner requester)
    {
        if (!territories.ContainsKey(name)) return;

        var data = territories[name];

        if (data.owner == requester)
        {
            territories[name] = (TerritoryOwner.None, 0);

            TerritoryPlane plane = FindPlaneByName(name);
            if (plane != null)
            {
                plane.ResetOwner();
            }
        }
    }

    public int GetFlagUsed(string name)
    {
        return territories.ContainsKey(name) ? territories[name].flagUsed : 0;
    }

    public TerritoryOwner GetOwner(string name)
    {
        return territories.ContainsKey(name) ? territories[name].owner : TerritoryOwner.None;
    }

    public Dictionary<string, (TerritoryOwner owner, int flagUsed)> GetAllTerritories()
    {
        return territories;
    }

    private TerritoryPlane FindPlaneByName(string name)
    {
        foreach (var plane in FindObjectsOfType<TerritoryPlane>())
        {
            if (plane.territoryName == name)
                return plane;
        }
        return null;
    }

    public TerritoryPlane GetCurrentTerritory(PlayerTag tag)
    {
        foreach (var plane in FindObjectsOfType<TerritoryPlane>())
        {
            if (tag == PlayerTag.Player1 && plane.player1Inside)
                return plane;

            if (tag == PlayerTag.Player2 && plane.player2Inside)
                return plane;
        }
        return null;
    }

    /// <summary>
    /// プレイヤーの合計スコアを返す
    /// </summary>
    public int GetScore(PlayerTag playerTag)
    {
        int total = 0;
        foreach (var kvp in territories)
        {
            var territoryName = kvp.Key;
            var owner = kvp.Value.owner;

            if ((playerTag == PlayerTag.Player1 && owner == TerritoryOwner.Player1) ||
                (playerTag == PlayerTag.Player2 && owner == TerritoryOwner.Player2))
            {
                if (territoryPoints.ContainsKey(territoryName))
                    total += territoryPoints[territoryName];
            }
        }
        return total;
    }

    /// <summary>
    /// 各エリアの点数一覧を取得
    /// </summary>
    public Dictionary<string, int> GetTerritoryPoints()
    {
        return territoryPoints;
    }
}
