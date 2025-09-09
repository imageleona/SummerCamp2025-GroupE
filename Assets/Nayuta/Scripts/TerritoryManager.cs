using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

    private Dictionary<TerritoryOwner, int> totalScores = new()
    {
        { TerritoryOwner.Player1, 0 },
        { TerritoryOwner.Player2, 0 }
    };

    [Header("エリア一覧 (シーンにあるTerritoryPlane)")]
    public TerritoryPlane[] territoryPlanes;

    [Header("通知UI")]
    [SerializeField] private NotificationUI notificationUI;

    void Start()
    {
        AssignRandomPoints();
    }

    void AssignRandomPoints()
    {
        List<int> pointsPool = new List<int>();

        pointsPool.AddRange(CreateList(5, 8));
        pointsPool.AddRange(CreateList(10, 7));
        pointsPool.AddRange(CreateList(15, 3));
        pointsPool.AddRange(CreateList(20, 2));
        pointsPool.AddRange(CreateList(50, 1));

        for (int i = 0; i < pointsPool.Count; i++)
        {
            int rand = Random.Range(i, pointsPool.Count);
            (pointsPool[i], pointsPool[rand]) = (pointsPool[rand], pointsPool[i]);
        }

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

    public bool TryCapture(string name, TerritoryOwner player, int flagCount)
    {
        if (!territories.ContainsKey(name))
        {
            territories[name] = (TerritoryOwner.None, 0);
        }

        var data = territories[name];

        if (data.owner == player)
        {
            return false;
        }

        if (data.owner == TerritoryOwner.None || flagCount > data.flagUsed)
        {
            if (territoryPoints.ContainsKey(name))
            {
                int gained = territoryPoints[name] * flagCount;
                totalScores[player] += gained;
                Debug.Log($"[TerritoryManager] {player} が {name} を獲得: {gained}点 (累計 {totalScores[player]}点)");

                if (notificationUI != null)
                {
                    string playerName = (player == TerritoryOwner.Player1) ? "Player1" : "Player2";
                    notificationUI.Show($"{playerName} が {name} ({gained}点)を{flagCount}本で獲得！");
                }
            }

            territories[name] = (player, flagCount);

            TerritoryPlane plane = FindPlaneByName(name);
            if (plane != null)
            {
                plane.SetOwner(player);
            }

            return true;
        }

        return false;
    }

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

    public int CountAreasOwnedBy(TerritoryOwner owner)
    {
        int count = 0;
        foreach (var kvp in territories)
        {
            if (kvp.Value.owner == owner)
            {
                count++;
            }
        }
        return count;
    }

    public int GetScore(TerritoryOwner owner)
    {
        return totalScores.ContainsKey(owner) ? totalScores[owner] : 0;
    }

    public Dictionary<string, int> GetTerritoryPoints()
    {
        return territoryPoints;
    }
}
