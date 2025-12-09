using UnityEngine;
using TMPro;
using System.Collections;

public class IntroSceneManager : MonoBehaviour
{
    [Header("Fade / UI")]
    public CanvasGroup fadeCanvas;
    public TMP_Text subtitleText;
    public TMP_Text objectiveText;

    [Header("Player / Camera")]
    public Transform playerRoot;         // PlayerCapsule (BODY)
    public Camera playerCamera;          // Main Camera (HEAD)

    [Header("Movement")]
    public Transform lookAtDoorTarget;
    public Transform doorStandPoint;
    public float rotateDuration = 2f;
    public float walkDuration = 2.2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip wakeAmbienceClip;
    public AudioClip doorRattleClip;

    [Header("Dialogue")]
    [TextArea] public string[] subtitleLines;
    public AudioClip[] voiceLines;

    [Header("Timings")]
    public float fadeDuration = 1.5f;
    public float defaultSubtitleDuration = 2f;
    public float objectiveVisibleTime = 5f;

    private PlayerLook playerLook;

    void Start()
    {
        playerLook = playerCamera.GetComponent<PlayerLook>();

        // Turn off camera movement during intro but DO NOT rotate camera
        if (playerLook != null)
            playerLook.canLook = false;

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        subtitleText.text = "";
        objectiveText.text = "";
        fadeCanvas.alpha = 1f;

        // Start ambience
        if (wakeAmbienceClip)
            audioSource.PlayOneShot(wakeAmbienceClip);

        // Fades from black only
        yield return StartCoroutine(FadeFromBlack());
        yield return new WaitForSeconds(1f);

        // Play all subtitles normally
        yield return StartCoroutine(PlaySubtitle(0));
        yield return StartCoroutine(PlaySubtitle(1));
        yield return StartCoroutine(PlaySubtitle(2));

        // Rotate ONLY the BODY
        yield return StartCoroutine(RotatePlayerBodyToDoor());
        yield return StartCoroutine(MovePlayerToDoor());

        if (doorRattleClip)
            audioSource.PlayOneShot(doorRattleClip);

        yield return StartCoroutine(PlaySubtitle(3));

        // Show objective
        objectiveText.text = "Find 3 clues to unlock the door.";
        yield return new WaitForSeconds(objectiveVisibleTime);
        objectiveText.text = "";

        // Re-enable looking
        if (playerLook != null)
            playerLook.canLook = true;
    }

    IEnumerator PlaySubtitle(int index)
    {
        subtitleText.text = subtitleLines[index];

        float duration = defaultSubtitleDuration;

        if (voiceLines != null && index < voiceLines.Length && voiceLines[index] != null)
        {
            audioSource.PlayOneShot(voiceLines[index]);
            duration = voiceLines[index].length + 0.25f;
        }

        yield return new WaitForSeconds(duration);
        subtitleText.text = "";
    }

    IEnumerator FadeFromBlack()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = 0f;
    }

    // Rotate BODY only (never camera)
    IEnumerator RotatePlayerBodyToDoor()
    {
        Quaternion start = playerRoot.rotation;
        Quaternion end = Quaternion.LookRotation(
            (lookAtDoorTarget.position - playerRoot.position).normalized);

        float t = 0f;
        while (t < rotateDuration)
        {
            t += Time.deltaTime;
            playerRoot.rotation = Quaternion.Slerp(start, end, t / rotateDuration);
            yield return null;
        }
    }

    IEnumerator MovePlayerToDoor()
    {
        Vector3 start = playerRoot.position;
        Vector3 end = doorStandPoint.position;

        float t = 0f;
        while (t < walkDuration)
        {
            t += Time.deltaTime;
            playerRoot.position = Vector3.Lerp(start, end, t / walkDuration);
            yield return null;
        }
    }
}
