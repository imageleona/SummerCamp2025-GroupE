using System.Collections.Generic;
using UnityEngine;

public enum TerritoryOwner
{
    None,
    Player1,
    Player2
}

public class TerritoryStatus
{
    public TerritoryOwner owner;
    public int flagUsed;
}

public class TerritoryManager : MonoBehaviour
{
    private Dictionary<string, TerritoryStatus> territoryStates = new();

    public bool TryCapture(string territoryName, TerritoryOwner newOwner, int flagToUse)
    {
        if (!territoryStates.ContainsKey(territoryName))
        {
            territoryStates[territoryName] = new TerritoryStatus
            {
                owner = newOwner,
                flagUsed = flagToUse
            };
            return true;
        }

        var current = territoryStates[territoryName];

        if (current.owner == TerritoryOwner.None)
        {
            territoryStates[territoryName].owner = newOwner;
            territoryStates[territoryName].flagUsed = flagToUse;
            return true;
        }

        if (current.owner != newOwner && flagToUse > current.flagUsed)
        {
            territoryStates[territoryName].owner = newOwner;
            territoryStates[territoryName].flagUsed = flagToUse;
            return true;
        }

        return false;
    }

    public bool TryRelease(string territoryName, TerritoryOwner owner)
    {
        if (!territoryStates.ContainsKey(territoryName)) return false;

        var current = territoryStates[territoryName];

        if (current.owner == owner)
        {
            // •úŠü‚µ‚Äƒtƒ‰ƒbƒO‚ð‰ñŽû
            current.owner = TerritoryOwner.None;
            current.flagUsed = 0;
            return true;
        }

        return false;
    }

    public TerritoryOwner GetOwner(string territoryName)
    {
        if (territoryStates.ContainsKey(territoryName))
        {
            return territoryStates[territoryName].owner;
        }
        return TerritoryOwner.None;
    }

    public int GetFlagUsed(string territoryName)
    {
        if (territoryStates.ContainsKey(territoryName))
        {
            return territoryStates[territoryName].flagUsed;
        }
        return 0;
    }

    public Dictionary<string, TerritoryStatus> GetAllTerritories()
    {
        return territoryStates;
    }
}
