using UnityEngine;
using TMPro;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    [Header("Fade / UI")]
    public CanvasGroup fadeCanvas;
    public TMP_Text subtitleText;
    public TMP_Text objectiveText;

    [Header("Player / Door")]
    public Transform playerRoot;
    public Transform lookAtDoorTarget;
    public Transform doorStandPoint;
    public float rotateDuration = 1.5f;
    public float walkDuration = 2f;

    [Header("Disable During Intro")]
    public Behaviour[] scriptsToDisable;

    [Header("Audio")]
    public AudioSource audioSource;          // ONE audio source only
    public AudioClip introMusic;             // 7-sec intro music
    public AudioClip doorRattleClip;         // door SFX

    [Header("Dialogue (4 lines total)")]
    [TextArea] public string[] subtitles;    // 4 subtitles
    public AudioClip[] voiceLines;           // 4 voice clips

    [Header("Timing")]
    public float fadeDuration = 3f;          // smooth fade
    public float afterFadeDelay = 0.5f;      // little pause before speech
    public float lineExtraDelay = 0.25f;     // padding after voice
    public float objectiveDisplayTime = 5f;

    // --------------------------------------------------------------
    void Start()
    {
        DisableControls();
        StartCoroutine(IntroFlow());
    }

    void DisableControls()
    {
        foreach (var s in scriptsToDisable)
            if (s != null) s.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void EnableControls()
    {
        foreach (var s in scriptsToDisable)
            if (s != null) s.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --------------------------------------------------------------
    IEnumerator IntroFlow()
    {
        fadeCanvas.alpha = 1f;
        subtitleText.text = "";
        objectiveText.text = "";

        // ---- 1. Fade + music ----
        if (introMusic != null)
        {
            audioSource.clip = introMusic;
            audioSource.Play();
        }

        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(afterFadeDelay);

        // ---- 2. First 3 lines near the bed ----
        yield return StartCoroutine(PlayLine(0));
        yield return StartCoroutine(PlayLine(1));
        yield return StartCoroutine(PlayLine(2));

        // ---- 3. Rotate + move to door ----
        yield return StartCoroutine(RotateToDoor());
        yield return StartCoroutine(WalkToDoor());

        // ---- 4. Door rattle SFX ----
        if (doorRattleClip != null)
            audioSource.PlayOneShot(doorRattleClip);

        yield return new WaitForSeconds(0.4f);

        // ---- 5. Final line at the door ----
        yield return StartCoroutine(PlayLine(3));

        // ---- 6. Show objective ----
        objectiveText.text = "Find 3 clues to unlock the door.";
        yield return new WaitForSeconds(objectiveDisplayTime);
        objectiveText.text = "";

        // ---- 7. Gameplay resumes ----
        EnableControls();
    }

    // --------------------------------------------------------------
    IEnumerator PlayLine(int index)
    {
        if (index >= subtitles.Length) yield break;

        subtitleText.text = subtitles[index];

        float duration = 2f;

        if (voiceLines != null && index < voiceLines.Length && voiceLines[index] != null)
        {
            audioSource.clip = voiceLines[index];
            audioSource.Play();
            duration = voiceLines[index].length + lineExtraDelay;
        }

        yield return new WaitForSeconds(duration);
        subtitleText.text = "";
    }

    IEnumerator FadeOut()
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

    IEnumerator RotateToDoor()
    {
        Quaternion startRot = playerRoot.rotation;
        Vector3 dir = (lookAtDoorTarget.position - playerRoot.position);
        dir.y = 0;

        Quaternion endRot = Quaternion.LookRotation(dir);

        float t = 0f;
        while (t < rotateDuration)
        {
            t += Time.deltaTime;
            playerRoot.rotation = Quaternion.Slerp(startRot, endRot, t / rotateDuration);
            yield return null;
        }
    }

    IEnumerator WalkToDoor()
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
        playerRoot.position = end;
    }
}
