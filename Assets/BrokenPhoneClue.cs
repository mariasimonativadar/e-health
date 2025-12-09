using UnityEngine;
using TMPro;
using System.Collections;

public class BrokenPhoneClue : MonoBehaviour
{
    [Header("Movement")]
    public Transform inspectPoint;       // Point in front of camera
    public float moveSpeed = 2f;
    public float returnSpeed = 2f;

    [Header("Dialogue")]
    public AudioSource audioSource;
    public AudioClip markoLine;
    public TMP_Text subtitleText;
    [TextArea] public string subtitleLine = "Shit… I must've broken it last night. I really drank too much…";
    public float subtitleDuration = 3f;

    [Header("Highlight")]
    public Renderer phoneRenderer;
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 2f;

    private bool triggered = false;
    private Vector3 originalPos;
    private Quaternion originalRot;

    private Material originalMat;
    private Color originalEmission;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;

        if (subtitleText != null)
            subtitleText.text = "";

        // Get original material/emission for highlight
        if (phoneRenderer != null)
        {
            originalMat = phoneRenderer.material;
            originalEmission = originalMat.GetColor("_EmissionColor");
        }
    }

    // ---------------- MOUSE INTERACTION ----------------
    void OnMouseEnter()
    {
        if (!triggered)
            EnableHighlight(true);
    }

    void OnMouseExit()
    {
        EnableHighlight(false);
    }

    void OnMouseDown()
    {
        if (!triggered)
            StartCoroutine(PhoneSequence());
    }

    // ---------------- HIGHLIGHT LOGIC ----------------
    void EnableHighlight(bool enable)
    {
        if (phoneRenderer == null || originalMat == null) return;

        if (enable)
        {
            originalMat.EnableKeyword("_EMISSION");
            originalMat.SetColor("_EmissionColor", highlightColor * highlightIntensity);
        }
        else
        {
            originalMat.SetColor("_EmissionColor", originalEmission);
        }
    }

    // ---------------- MAIN SEQUENCE ----------------
    IEnumerator PhoneSequence()
    {
        triggered = true;
        EnableHighlight(false);

        // Move TO inspect point
        yield return StartCoroutine(MoveObject(transform, originalPos, inspectPoint.position, originalRot, inspectPoint.rotation, moveSpeed));

        // Play dialogue + subtitles
        if (subtitleText) subtitleText.text = subtitleLine;
        float wait = subtitleDuration;

        if (audioSource && markoLine)
        {
            audioSource.PlayOneShot(markoLine);
            wait = markoLine.length + 0.2f;
        }

        yield return new WaitForSeconds(wait);

        if (subtitleText) subtitleText.text = "";

        // Register clue → ClueTracker handles lights + door unlocking
        ClueTracker tracker = FindObjectOfType<ClueTracker>();
        if (tracker != null)
            tracker.RegisterClue();

        // Move BACK to start
        yield return StartCoroutine(MoveObject(transform, inspectPoint.position, originalPos, inspectPoint.rotation, originalRot, returnSpeed));

        // Destroy or disable?  
        // Destroy(gameObject); // If you want it removed. Otherwise keep it.
    }

    // ---------------- MOVEMENT HELPER ----------------
    IEnumerator MoveObject(Transform obj, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float speed)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            obj.position = Vector3.Lerp(startPos, endPos, t);
            obj.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
    }
}
