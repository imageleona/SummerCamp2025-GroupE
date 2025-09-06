using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class TerritoryPlane : MonoBehaviour
{
    public string territoryName = "Territory";
    public bool player1Inside = false;
    public bool player2Inside = false;
    public TerritoryOwner owner = TerritoryOwner.None;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null && rend.material != null)
        {
            rend.material.color = Color.white;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1Inside = true;
            Debug.Log($"[TerritoryPlane] Player1 entered {territoryName}");
        }
        else if (other.CompareTag("Player2"))
        {
            player2Inside = true;
            Debug.Log($"[TerritoryPlane] Player2 entered {territoryName}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1Inside = false;
            Debug.Log($"[TerritoryPlane] Player1 exited {territoryName}");
        }
        else if (other.CompareTag("Player2"))
        {
            player2Inside = false;
            Debug.Log($"[TerritoryPlane] Player2 exited {territoryName}");
        }
    }

    public void SetOwner(TerritoryOwner newOwner)
    {
        Debug.Log($"[TerritoryPlane] SetOwner called: {territoryName} Å® {newOwner}");
        owner = newOwner;
        UpdateColor();
    }

    public void ResetOwner()
    {
        owner = TerritoryOwner.None;
        if (rend != null && rend.material != null)
        {
            rend.material.color = Color.white;
        }
    }

    private void UpdateColor()
    {
        Debug.Log($"[TerritoryPlane] UpdateColor called for {territoryName}, owner: {owner}");

        if (rend == null || rend.material == null) return;

        Color newColor = Color.white;

        if (owner == TerritoryOwner.Player1)
            newColor = Color.blue;
        else if (owner == TerritoryOwner.Player2)
            newColor = Color.red;

        newColor.a = 1.0f; // äÆëSïsìßñæ

        rend.material.color = newColor;

        Debug.Log($"[TerritoryPlane] '{territoryName}' color updated to {newColor} (owner: {owner})");
    }
}
