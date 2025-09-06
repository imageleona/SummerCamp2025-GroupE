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
}
