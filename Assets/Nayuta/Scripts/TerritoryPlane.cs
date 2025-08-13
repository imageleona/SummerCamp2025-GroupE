using UnityEngine;

public class TerritoryPlane : MonoBehaviour
{
    public int territoryIndex;
    private Renderer rend;
    private TerritoryManager manager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.material); // マテリアル共有回避
        manager = FindObjectOfType<TerritoryManager>();
    }

    void Update()
    {
        UpdateColor();  // 毎フレーム色を確認して更新
    }

    void UpdateColor()
    {
        TerritoryOwner owner = manager.GetOwner(territoryIndex);

        if (owner == TerritoryOwner.Player1)
        {
            rend.material.color = Color.blue;
        }
        else if (owner == TerritoryOwner.Player2)
        {
            rend.material.color = Color.red;
        }
        else
        {
            rend.material.color = Color.white;
        }
    }

    // 以下の Trigger 処理（プレイヤーが中にいるか）もそのまま残してOK
    public bool player1Inside = false;
    public bool player2Inside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1")) player1Inside = true;
        if (other.CompareTag("Player2")) player2Inside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1")) player1Inside = false;
        if (other.CompareTag("Player2")) player2Inside = false;
    }
}
