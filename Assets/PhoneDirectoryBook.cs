using UnityEngine;

public class PhoneDirectoryBook : MonoBehaviour
{
    [Header("Directory UI")]
    public GameObject directoryPanel;        // beige phone directory panel

    [Header("Player Distance Check")]
    public Transform player;
    public float interactDistance = 2f;

    [Header("Highlight")]
    public Renderer targetRenderer;          // book mesh renderer
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f); // soft yellow

    private Color originalEmissionColor;
    private bool hasOriginalEmission = false;

    void Start()
    {
        if (directoryPanel != null)
            directoryPanel.SetActive(false);

        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null && targetRenderer.material.HasProperty("_EmissionColor"))
        {
            originalEmissionColor = targetRenderer.material.GetColor("_EmissionColor");
            hasOriginalEmission = true;
        }
    }

    // =========================
    //  CLICK TO OPEN DIRECTORY
    // =========================
    void OnMouseDown()
    {
        if (!PlayerIsClose()) return;

        if (directoryPanel != null)
            directoryPanel.SetActive(true);

        // 🔓 Unlock the phone for use
        PhoneLogic.directoryUnlocked = true;
        Debug.Log("Directory opened → Phone unlocked");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Call from a Close button if you add one
    public void CloseDirectory()
    {
        if (directoryPanel != null)
            directoryPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
