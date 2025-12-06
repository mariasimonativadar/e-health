using UnityEngine;
using TMPro;

public class UnderlayPulse : MonoBehaviour
{
    private TMP_Text tmpText;
    private Material matInstance;

    public Color glowColor = new Color(0.11f, 0.36f, 0.24f, 1f); // bottle green
    public float minAlpha = 0.2f;
    public float maxAlpha = 0.7f;
    public float pulseSpeed = 2f;

    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        matInstance = tmpText.fontMaterial; // get material instance
    }

    void Update()
    {
        // make the glow pulse over time
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        Color pulsing = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);

        matInstance.SetColor(ShaderUtilities.ID_UnderlayColor, pulsing);
        matInstance.SetFloat(ShaderUtilities.ID_UnderlaySoftness, 0.6f);
        matInstance.SetFloat(ShaderUtilities.ID_UnderlayOffsetY, -0.2f);
    }
}

