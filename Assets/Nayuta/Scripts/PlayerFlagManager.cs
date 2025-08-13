using System.Collections.Generic;
using UnityEngine;

public enum PlayerTag
{
    Player1,
    Player2
}

public class PlayerFlagManager : MonoBehaviour
{
    public int initialFlags = 10;
    private Dictionary<PlayerTag, int> flagCounts = new();

    void Awake()
    {
        flagCounts[PlayerTag.Player1] = initialFlags;
        flagCounts[PlayerTag.Player2] = initialFlags;
    }

    public int GetFlags(PlayerTag player)
    {
        return flagCounts[player];
    }

    public bool ConsumeFlags(PlayerTag player, int amount)
    {
        if (flagCounts[player] >= amount)
        {
            flagCounts[player] -= amount;
            return true;
        }
        return false;
    }

    public void AddFlags(PlayerTag player, int amount)
    {
        flagCounts[player] += amount;
    }

    public void PrintAllFlagCounts()
    {
        Debug.Log("=== Player Flag Counts ===");
        foreach (var kvp in flagCounts)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value} flags");
        }
    }
}
