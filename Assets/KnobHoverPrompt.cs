using UnityEngine;
using TMPro;

public class KnobHoverPrompt : MonoBehaviour
{
    [Header("Prompt UI")]
    public TMP_Text promptText;
    public float fadeSpeed = 4f;
    [TextArea]
    public string hintMessage = "Turn the dial… until the voice feels right.";

    [Header("Highlight")]
    public Renderer knobRenderer;
    public Color highlightColor = new Color(0.3f, 0.6f, 1f);

    private Color baseColor;
    private float targetAlpha = 0f;

    private void Awake()
    {
        if (promptText != null)
        {
            Color c = promptText.color;
            c.a = 0f;                 // start invisible
            promptText.color = c;
        }

        if (knobRenderer != null)
        {
            baseColor = knobRenderer.material.color;
        }
    }

    private void Update()
    {
        if (promptText == null) return;

        Color c = promptText.color;
        c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
        promptText.color = c;
    }

    private void OnMouseEnter()
    {
        targetAlpha = 1f;

        if (promptText != null)
        {
            promptText.text = hintMessage;
        }

        if (knobRenderer != null)
        {
            knobRenderer.material.color = highlightColor;
        }
    }

    private void OnMouseExit()
    {
        targetAlpha = 0f;

        if (knobRenderer != null)
        {
            knobRenderer.material.color = baseColor;
        }
    }
}
