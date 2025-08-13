using UnityEngine;

public enum TerritoryOwner
{
    None,
    Player1,
    Player2
}

public class TerritoryManager : MonoBehaviour
{
    public int numberOfTerritories = 5;

    private TerritoryOwner[] territoryOwners;
    private int[] flagCosts;

    void Awake()
    {
        territoryOwners = new TerritoryOwner[numberOfTerritories];
        flagCosts = new int[numberOfTerritories];

        for (int i = 0; i < numberOfTerritories; i++)
        {
            territoryOwners[i] = TerritoryOwner.None;
            flagCosts[i] = 0;
        }
    }

    public bool CanCapture(int index, TerritoryOwner challenger, int flagUsed)
    {
        if (territoryOwners[index] == challenger)
            return false;

        int currentCost = flagCosts[index];
        return flagUsed > currentCost;
    }

    public void CaptureTerritory(int index, TerritoryOwner newOwner, int flagUsed)
    {
        territoryOwners[index] = newOwner;
        flagCosts[index] = flagUsed;
        Debug.Log($"Territory {index} is now owned by {newOwner} with {flagUsed} flags");
    }

    public bool TryCapture(int index, TerritoryOwner challenger, int flagUsed)
    {
        var flagManager = FindObjectOfType<PlayerFlagManager>();
        var playerTag = (challenger == TerritoryOwner.Player1) ? PlayerTag.Player1 : PlayerTag.Player2;

        if (!CanCapture(index, challenger, flagUsed))
        {
            Debug.Log("条件不足：フラッグ本数が少ない or 既に自分のもの");
            return false;
        }

        if (!flagManager.ConsumeFlags(playerTag, flagUsed))
        {
            Debug.Log("フラッグ不足！");
            return false;
        }

        CaptureTerritory(index, challenger, flagUsed);
        return true;
    }

    public TerritoryOwner GetOwner(int index)
    {
        return territoryOwners[index];
    }

    public int GetFlagCost(int index)
    {
        return flagCosts[index];
    }
}
