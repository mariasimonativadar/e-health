using UnityEngine;
using TMPro;

public class PhoneDirectoryBook : MonoBehaviour
{
    [Header("Directory UI")]
    public PhoneDirectoryPanel directoryPanel;   // beige phone directory panel controller
    public TMP_Text directoryText;              // Text inside the panel listing numbers

    [Header("Directory Content")]
    [TextArea(3, 10)]
    public string directoryContent;             // <-- you type the text here in Inspector

    [Header("Player Distance Check")]
    public Transform player;
    public float interactDistance = 2f;

    [Header("Highlight")]
    public Renderer targetRenderer;             // book mesh renderer
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f); // soft yellow

    private Color originalEmissionColor;
    private bool hasOriginalEmission = false;

    void Start()
    {
        if (directoryPanel != null)
            directoryPanel.CloseImmediate();    // make sure it's hidden at start

        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null && targetRenderer.material.HasProperty("_EmissionColor"))
        {
            originalEmissionColor = targetRenderer.material.GetColor("_EmissionColor");
            hasOriginalEmission = true;
        }

        // 🔹 Auto-fill from Inspector string instead of hard-coded text
        if (directoryText != null && !string.IsNullOrEmpty(directoryContent))
        {
            directoryText.richText = true;  
            directoryText.text = directoryContent;
        }
    }

    // =========================
    //  CLICK TO OPEN DIRECTORY
    // =========================
    void OnMouseDown()
    {
        if (!PlayerIsClose()) return;

        if (directoryPanel != null)
            directoryPanel.Open();             // uses PhoneDirectoryPanel logic

        // 🔓 Unlock the phone for use
        PhoneLogic.directoryUnlocked = true;
        Debug.Log("Directory opened → Phone unlocked");
    }

    // Call from a Close button if you add one
    public void CloseDirectory()
    {
        if (directoryPanel != null)
            directoryPanel.Close();
    }

    // =========================
    //  HOVER HIGHLIGHT
    // =========================
    void OnMouseEnter()
    {
        if (!PlayerIsClose()) return;
        SetHighlight(true);
    }

    void OnMouseExit()
    {
        SetHighlight(false);
    }

    void SetHighlight(bool on)
    {
        if (!hasOriginalEmission || targetRenderer == null)
            return;

        var mat = targetRenderer.material;

        if (on)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", highlightColor);
        }
        else
        {
            mat.SetColor("_EmissionColor", originalEmissionColor);
        }
    }

    // =========================
    //  HELPER
    // =========================
    bool PlayerIsClose()
    {
        if (player == null) return true;
        return Vector3.Distance(player.position, transform.position) <= interactDistance;
    }
}
