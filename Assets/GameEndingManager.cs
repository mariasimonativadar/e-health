using UnityEngine;
using TMPro;
using System.Collections;

public class GameEndingManager : MonoBehaviour
{
    [Header("Fade Message UI")]
    public CanvasGroup fadeMessageCanvas;
    public TMP_Text messageText;
    public string initialMessage = "A few minutes later...";
    public string endMessage = "THE END";

    [Header("Door / Light")]
    public EndingDoor endingDoor;      // <-- FINAL DOOR SCRIPT HERE
    public Light brightLight;
    public float lightTargetIntensity = 10f;
    public float lightLerpDuration = 3f;

    [Header("Player / Camera")]
    public Transform playerRoot;
    public Transform lookAtDoorTarget;
    public Transform doorStandPoint;
    public float cameraTurnDuration = 2.5f;
    public float moveToDoorDuration = 3f;
    public Behaviour[] playerControlScripts;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip doorbellClip;
    public AudioClip miaPanicClip;   // 5 sec
    public AudioClip momVoiceClip;   // 13 sec
    public AudioClip softEndMusic;

    [Header("Timing")]
    public float delayBeforeMessage = 1f;
    public float messageDisplayTime = 2f;
    public float delayAfterMessage = 1f;
    public float delayAfterDoorbell = 1f;
    public float delayBeforeEndText = 2f;

    public void StartEndingSequence()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // Disable player movement scripts
        foreach (var c in playerControlScripts)
            if (c) c.enabled = false;

        // -- Fade Message --
        yield return new WaitForSeconds(delayBeforeMessage);

        messageText.text = initialMessage;
        fadeMessageCanvas.alpha = 1;
        yield return new WaitForSeconds(messageDisplayTime);
        fadeMessageCanvas.alpha = 0;

        yield return new WaitForSeconds(delayAfterMessage);

        // -- Doorbell --
        audioSource.PlayOneShot(doorbellClip);
        yield return new WaitForSeconds(delayAfterDoorbell);

        // -- Rotate Player --
        yield return StartCoroutine(RotatePlayerToDoor());

        // -- Move Player --
        yield return StartCoroutine(MovePlayerToDoor());

        // -- Mia Dialogue --
        audioSource.PlayOneShot(miaPanicClip);
        yield return new WaitForSeconds(miaPanicClip.length + 0.3f);

        // -- Mom Dialogue --
        audioSource.PlayOneShot(momVoiceClip);
        yield return new WaitForSeconds(momVoiceClip.length + 0.3f);

        // -- Door Opens only AFTER Mom finishes --
        if (endingDoor != null)
            endingDoor.OpenDoor();

        // -- Light Brightens --
        if (brightLight != null)
            yield return StartCoroutine(LerpLight());

        // Wait then show END
        yield return new WaitForSeconds(delayBeforeEndText);

        messageText.text = endMessage;
        fadeMessageCanvas.alpha = 1;

        // Ending music
        if (softEndMusic != null)
            audioSource.PlayOneShot(softEndMusic);
    }

    // Smooth rotation toward door
    IEnumerator RotatePlayerToDoor()
    {
        Quaternion startRot = playerRoot.rotation;
        Quaternion endRot = Quaternion.LookRotation(
            lookAtDoorTarget.position - playerRoot.position
        );

        float t = 0;
        while (t < cameraTurnDuration)
        {
            t += Time.deltaTime;
            playerRoot.rotation = Quaternion.Slerp(startRot, endRot, t / cameraTurnDuration);
            yield return null;
        }
    }

    // Move smoothly to standing point
    IEnumerator MovePlayerToDoor()
    {
        Vector3 start = playerRoot.position;
        Vector3 end = doorStandPoint.position;

        float t = 0;
        while (t < moveToDoorDuration)
        {
            t += Time.deltaTime;
            playerRoot.position = Vector3.Lerp(start, end, t / moveToDoorDuration);
            yield return null;
        }
    }

    // Light fade-in
    IEnumerator LerpLight()
    {
        float start = brightLight.intensity;
        float t = 0;

        while (t < lightLerpDuration)
        {
            t += Time.deltaTime;
            brightLight.intensity = Mathf.Lerp(start, lightTargetIntensity, t / lightLerpDuration);
            yield return null;
        }
    }
}
