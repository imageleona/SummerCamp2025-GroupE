using UnityEngine;
using TMPro;

public class StatusDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText; // UnityエディタでUI Textをここにドラッグする

    void Update()
    {
        TerritoryManager territoryManager = FindObjectOfType<TerritoryManager>();
        PlayerFlagManager flagManager = FindObjectOfType<PlayerFlagManager>();

        string output = "=== Flag Status ===\n";
        output += $"Player1: {flagManager.GetFlags(PlayerTag.Player1)} flags\n";
        output += $"Player2: {flagManager.GetFlags(PlayerTag.Player2)} flags\n\n";

        output += "=== Territories ===\n";
        for (int i = 0; i < territoryManager.numberOfTerritories; i++)
        {
            var owner = territoryManager.GetOwner(i);
            var cost = territoryManager.GetFlagCost(i);
            output += $"Territory {i}: {owner}, {cost} flags\n";
        }

        displayText.text = output;
    }
}