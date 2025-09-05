using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Renderer))]
public class TerritoryPlane : MonoBehaviour
{
    public string territoryName = "Area_0";

    [HideInInspector] public bool player1Inside = false;
    [HideInInspector] public bool player2Inside = false;

    private Renderer rend;
    private TerritoryOwner owner = TerritoryOwner.None;

    void Awake()
    {
        rend = GetComponent<Renderer>();

        // 透明シェーダーにしておく（Standard または URP の Transparent）
        SetTransparent();
    }

    void Update()
    {
        UpdateColor();
    }

    void SetTransparent()
    {
        if (rend.material.HasProperty("_Mode"))
        {
            // 標準シェーダーなら RenderingMode = Transparent に（※必要に応じて）
            rend.material.SetFloat("_Mode", 3); // 3 = Transparent
        }

        // Enable transparency blending
        rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rend.material.SetInt("_ZWrite", 0);
        rend.material.DisableKeyword("_ALPHATEST_ON");
        rend.material.EnableKeyword("_ALPHABLEND_ON");
        rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        rend.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    public void SetOwner(TerritoryOwner newOwner)
    {
        owner = newOwner;
        UpdateColor();
    }

    void UpdateColor()
    {
        if (owner == TerritoryOwner.None)
        {
            rend.material.color = new Color(1f, 1f, 1f, 0f); // 完全に透明
        }
        else if (owner == TerritoryOwner.Player1)
        {
            rend.material.color = new Color(0.2f, 0.5f, 1f, 1f); // 青
        }
        else if (owner == TerritoryOwner.Player2)
        {
            rend.material.color = new Color(1f, 0.3f, 0.3f, 1f); // 赤
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1Inside = true;
        }
        else if (other.CompareTag("Player2"))
        {
            player2Inside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            player1Inside = false;
        }
        else if (other.CompareTag("Player2"))
        {
            player2Inside = false;
        }
    }
}
