using UnityEngine;
using System.Collections;
using TMPro;

public class BottleInspect : MonoBehaviour
{
    [Header("References")]
    public Transform inspectPosition;
    public GameObject brokenBottlePrefab;
    public AudioClip glassBreakSound;
    public TextMeshPro hiddenText;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;
    public float fadeInTime = 2f;
    public float shakeDuration = 1.5f;
    public float shakeIntensity = 0.05f;
    public float fallForce = 2.5f;
    public float fallDelay = 0.5f;

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isInspecting = false;
    private bool canBreak = false;
    private bool hasBroken = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Exposed to other scripts
    public bool IsInspecting => isInspecting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (hiddenText != null)
        {
            Color c = hiddenText.color;
            c.a = 0f;
            hiddenText.color = c;
        }
    }

    public void TriggerBottleEvent()
    {
        // Disable highlight when bottle is clicked
        BottleOutline outline = GetComponent<BottleOutline>();
        if (outline != null)
            outline.DisableOutline();

        if (!isInspecting)
            StartCoroutine(InspectSequence());
    }

    IEnumerator InspectSequence()
    {
        isInspecting = true;

        rb.isKinematic = true;
        rb.useGravity = false;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Move to inspect position
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(originalPosition, inspectPosition.position, t);
            yield return null;
        }

        // Rotate bottle upright for reveal
        Quaternion targetRot = Quaternion.Euler(-90f, 90f, 0f);
        float rotT = 0f;
        while (rotT < 1f)
        {
            rotT += Time.deltaTime * rotateSpeed;
            transform.rotation = Quaternion.Slerp(originalRotation, targetRot, rotT);
            yield return null;
        }

        // Fade in text
        if (hiddenText != null)
        {
            float fade = 0f;
            while (fade < 1f)
            {
                fade += Time.deltaTime / fadeInTime;
                Color c = hiddenText.color;
                c.a = Mathf.Lerp(0f, 1f, fade);
                hiddenText.color = c;
                yield return null;
            }
        }

        // Shake bottle
        yield return StartCoroutine(ShakeBottle());

        // Delay then fall
        yield return new WaitForSeconds(fallDelay);
        StartCoroutine(FallAndBreak());
    }

    IEnumerator ShakeBottle()
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = originalPos + Random.insideUnitSphere * shakeIntensity;
            yield return null;
        }

        transform.position = originalPos;
    }

    IEnumerator FallAndBreak()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        canBreak = true;

        // Push bottle down for falling animation
        rb.AddForce(Vector3.down * fallForce, ForceMode.Impulse);

        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canBreak || hasBroken) return;

        hasBroken = true;

        // --- Spawn broken bottle lying flat ---
        if (brokenBottlePrefab)
        {
            Quaternion flatRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            Instantiate(brokenBottlePrefab, transform.position, flatRotation);
        }

        // --- Play glass break sound fully ---
        if (glassBreakSound)
        {
            GameObject audioObj = new GameObject("GlassBreakSound");
            AudioSource src = audioObj.AddComponent<AudioSource>();
            src.clip = glassBreakSound;
            src.Play();
            Destroy(audioObj, glassBreakSound.length);
        }

        Destroy(gameObject);
    }
}
