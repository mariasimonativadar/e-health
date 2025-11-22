using UnityEngine;

public class BottleReveal : MonoBehaviour
{
    [Header("References")]
    public Renderer labelRenderer; // Assign the HiddenLabel MeshRenderer here

    [Header("Settings")]
    [Range(0f, 1f)] public float hiddenAlpha = 0f;  // When lying down
    [Range(0f, 1f)] public float visibleAlpha = 0.7f; // When upright
    public float revealThreshold = 0.7f; // How vertical bottle must be to reveal text

    private Material labelMaterial;
    private Color baseColor;

    void Start()
    {
        if (labelRenderer != null)
        {
            labelMaterial = labelRenderer.material;
            baseColor = labelMaterial.color;

            // Start hidden
            SetAlpha(hiddenAlpha);
        }
    }

    void Update()
    {
        // Check bottle's "uprightness" — 1 = fully up, 0 = flat
        float uprightDot = Vector3.Dot(transform.up, Vector3.up);

        if (labelMaterial != null)
        {
            // If bottle is mostly upright, fade in; else fade out
            if (uprightDot > revealThreshold)
                SetAlpha(Mathf.Lerp(labelMaterial.color.a, visibleAlpha, Time.deltaTime * 5f));
            else
                SetAlpha(Mathf.Lerp(labelMaterial.color.a, hiddenAlpha, Time.deltaTime * 5f));
        }
    }

    void SetAlpha(float alpha)
    {
        baseColor.a = alpha;
        labelMaterial.color = baseColor;
    }
}
