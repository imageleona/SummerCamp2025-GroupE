using UnityEngine;

public class TerritoryPlane : MonoBehaviour
{
    public int territoryIndex;
    private Renderer rend;
    private TerritoryManager manager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.material); // �}�e���A�����L���
        manager = FindObjectOfType<TerritoryManager>();
    }

    void Update()
    {
        UpdateColor();  // ���t���[���F���m�F���čX�V
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

    // �ȉ��� Trigger �����i�v���C���[�����ɂ��邩�j�����̂܂܎c����OK
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
