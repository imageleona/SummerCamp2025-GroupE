using UnityEngine;

public class TerritoryPlane : MonoBehaviour
{
    public int territoryIndex;                      // Managerで使う識別番号
    public TerritoryManager territoryManager;       // Inspectorで設定する

    private float stayTime = 0f;
    private bool isPlayerInside = false;
    private bool alreadyCaptured = false;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // マテリアル共有を避ける
            rend.material = new Material(rend.material);
        }
    }

    void Update()
    {
        if (isPlayerInside && !alreadyCaptured)
        {
            stayTime += Time.deltaTime;

            if (stayTime >= 5f)
            {
                territoryManager.CaptureTerritory(territoryIndex);
                alreadyCaptured = true;

                if (rend != null)
                    rend.material.color = Color.blue;

                Debug.Log($"Territory {territoryIndex} captured!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            stayTime = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            stayTime = 0f;
        }
    }
}
