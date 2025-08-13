using UnityEngine;

public class FlagSelectionInput : MonoBehaviour
{
    private int selectedFlags = 1;

    void Update()
    {
        // Select flag count with number keys 1 to 9
        for (KeyCode k = KeyCode.Alpha1; k <= KeyCode.Alpha9; k++)
        {
            if (Input.GetKeyDown(k))
            {
                selectedFlags = k - KeyCode.Alpha0;
                Debug.Log($"Selected flags: {selectedFlags}");
            }
        }

        // Player1 attempts capture with F key
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryCaptureFromPlayer(TerritoryOwner.Player1, PlayerTag.Player1);
        }

        // Player2 attempts capture with J key
        if (Input.GetKeyDown(KeyCode.J))
        {
            TryCaptureFromPlayer(TerritoryOwner.Player2, PlayerTag.Player2);
        }
    }

    void TryCaptureFromPlayer(TerritoryOwner owner, PlayerTag tag)
    {
        TerritoryPlane[] planes = FindObjectsOfType<TerritoryPlane>();
        TerritoryManager territoryManager = FindObjectOfType<TerritoryManager>();
        PlayerFlagManager flagManager = FindObjectOfType<PlayerFlagManager>();

        foreach (var plane in planes)
        {
            bool inside = (owner == TerritoryOwner.Player1) ? plane.player1Inside : plane.player2Inside;
            if (inside)
            {
                bool success = territoryManager.TryCapture(plane.territoryIndex, owner, selectedFlags);

                if (success)
                {
                    Debug.Log($"{owner} captured territory {plane.territoryIndex} with {selectedFlags} flags");
                }
                else
                {
                    Debug.Log($"{owner} failed to capture territory {plane.territoryIndex}");
                }

                PrintTerritoryState(territoryManager);
                flagManager.PrintAllFlagCounts();
                break;
            }
        }
    }

    void PrintTerritoryState(TerritoryManager manager)
    {
        Debug.Log("=== Territory Ownership Status ===");
        for (int i = 0; i < manager.numberOfTerritories; i++)
        {
            var owner = manager.GetOwner(i);
            var cost = manager.GetFlagCost(i);
            Debug.Log($"Territory {i}: {owner} - {cost} flags");
        }
    }
}
