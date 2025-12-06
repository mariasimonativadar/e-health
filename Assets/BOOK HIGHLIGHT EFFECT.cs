using UnityEngine;

public class HighlightOnLook : MonoBehaviour
{
    public Renderer targetRenderer;
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);
    private Color originalColor;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        originalColor = targetRenderer.material.color;
    }

    public void OnHoverEnter()
    {
        targetRenderer.material.SetColor("_EmissionColor", highlightColor * 1.3f);
    }

    public void OnHoverExit()
    {
        targetRenderer.material.SetColor("_EmissionColor", Color.black);
    }
}
