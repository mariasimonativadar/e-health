using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    public Renderer rend;
    public Color highlightColor = Color.yellow;
    public float intensity = 2f;

    private Color originalEmission;
    private Material mat;

    void Start()
    {
        mat = rend.material;
        originalEmission = mat.GetColor("_EmissionColor");
    }

    void OnMouseEnter()
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", highlightColor * intensity);
    }

    void OnMouseExit()
    {
        mat.SetColor("_EmissionColor", originalEmission);
    }
}
