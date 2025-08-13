using UnityEngine;

public class TerritoryManager : MonoBehaviour
{
    public int numberOfTerritories = 4;
    private bool[] territoryOwned;

    void Awake()
    {
        territoryOwned = new bool[numberOfTerritories];
    }

    public void CaptureTerritory(int index)
    {
        if (!territoryOwned[index])
        {
            territoryOwned[index] = true;
            Debug.Log($"[Manager] Territory {index} is now owned.");
        }
    }

    public bool IsTerritoryOwned(int index)
    {
        return territoryOwned[index];
    }
}
