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
    public EndingDoor endingDoor;      
    public Light brightLight;
    public float lightTargetIntensity = 10f;
    public float lightLerpDuration = 2.5f;

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
    public AudioClip miaPanicClip;
    public AudioClip momVoiceClip;
    public AudioClip softEndMusic;

    [Header("Timing")]
    public float delayBeforeMessage = 1f;
    public float messageDisplayTime = 2f;
    public float delayAfterMessage = 1f;
    public float delayAfterDoorbell = 1f;
    public float delayBeforeEndText = 1.5f;

    public void StartEndingSequence()
    {
        StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {
        // Disable player movement
        foreach (var c in playerControlScripts)
            if (c) c.enabled = false;

        // FADE-IN INITIAL MESSAGE
        yield return new WaitForSeconds(delayBeforeMessage);
        messageText.text = initialMessage;
        fadeMessageCanvas.alpha = 1;
        yield return new WaitForSeconds(messageDisplayTime);

        // FADE OUT MESSAGE
        fadeMessageCanvas.alpha = 0;
        yield return new WaitForSeconds(delayAfterMessage);

        // DOORBELL
        audioSource.PlayOneShot(doorbellClip);
        yield return new WaitForSeconds(delayAfterDoorbell);

        // TURN PLAYER
        yield return StartCoroutine(RotatePlayerToDoor());

        // MOVE PLAYER TO DOOR
        yield return StartCoroutine(MovePlayerToDoor());

        // MIA LINE
        audioSource.PlayOneShot(miaPanicClip);
        yield return new WaitForSeconds(miaPanicClip.length + 0.3f);

        // MOM LINE
        audioSource.PlayOneShot(momVoiceClip);
        yield return new WaitForSeconds(momVoiceClip.length + 0.3f);

        // DOOR OPENS
        if (endingDoor != null)
            endingDoor.OpenDoor();

        // BRIGHT LIGHT FADES IN
        if (brightLight != null)
            yield return StartCoroutine(LerpLight());

        // WAIT BEFORE THE END MESSAGE
        yield return new WaitForSeconds(delayBeforeEndText);

        // SHOW THE END WITH NICE FADE
        messageText.text = endMessage;
        StartCoroutine(FadeInEndMessage());

        if (softEndMusic != null)
            audioSource.PlayOneShot(softEndMusic);
    }

    // Fade-in for final END text
    IEnumerator FadeInEndMessage()
    {
        fadeMessageCanvas.alpha = 0;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            fadeMessageCanvas.alpha = t;
            yield return null;
        }
    }

    // Rotate camera toward door
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

    // Move player to door
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

    // Bright light fades in through door
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
