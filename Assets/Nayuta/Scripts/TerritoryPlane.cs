using UnityEngine;

public class TerritoryPlane : MonoBehaviour
{
    [Tooltip("このエリアの名前（ユニークな名称をInspectorで設定）")]
    public string territoryName;

    public bool player1Inside = false;
    public bool player2Inside = false;

    private Renderer rend;
    private TerritoryManager manager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(rend.material); // マテリアル共有を防ぐ
        }

        manager = FindObjectOfType<TerritoryManager>();
    }

    void Update()
    {
        UpdateColor();
    }

    void UpdateColor()
    {
        if (manager == null || rend == null || string.IsNullOrEmpty(territoryName)) return;

        TerritoryOwner owner = manager.GetOwner(territoryName);

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
            // 未取得なら透明
            rend.material.color = new Color(1f, 1f, 1f, 0f);
        }
    }

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
