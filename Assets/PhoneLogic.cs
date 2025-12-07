using UnityEngine;
using TMPro;
using System.Collections;

public class PhoneLogic : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text displayText;
    private string currentInput = "";

    [Header("Dial Settings")]
    public string supportiveNumber = "5551834";   // main number for the story
    public int maxDigits = 7;

    // Set true by PhoneDirectoryBook when the directory is opened
    public static bool directoryUnlocked = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip digitBeep;
    public AudioClip deleteBeep;
    public AudioClip connectClip;
    public AudioClip wrongNumberClip;

    [Header("Supportive Call Audio (dialogue sequence)")]
    public AudioClip[] supportiveCallLines;

    [Header("Subtitles for Supportive Call")]
    public GameObject subtitlesPanel;
    public TMP_Text subtitlesText;
    public string[] supportiveSubtitleLines;
    public float subtitleLineDuration = 3f;

    private bool isCalling = false;

    void Start()
    {
        currentInput = "";
        if (displayText != null) displayText.text = "";
        if (subtitlesPanel) subtitlesPanel.SetActive(false);

        directoryUnlocked = false; // always reset on scene load
    }

    // ============================================================
    // ADD DIGIT
    // ============================================================
    public void AddDigit(string digit)
    {
        if (!directoryUnlocked)
        {
            Debug.Log("Digit ignored: directory not unlocked yet.");
            return;
        }

        if (isCalling) return;
        if (currentInput.Length >= maxDigits) return;

        currentInput += digit;

        if (displayText != null)
            displayText.text = currentInput;

        if (audioSource && digitBeep)
            audioSource.PlayOneShot(digitBeep);
    }

    // ============================================================
    // DELETE DIGIT
    // ============================================================
    public void DeleteDigit()
    {
        if (!directoryUnlocked) return;
        if (isCalling) return;

        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);

            if (displayText != null)
                displayText.text = currentInput;

            if (audioSource && deleteBeep)
                audioSource.PlayOneShot(deleteBeep);
        }
    }

    // ============================================================
    // CALL BUTTON
    // ============================================================
    public void CallButton()
    {
        if (!directoryUnlocked) return;
        if (isCalling) return;

        if (currentInput == supportiveNumber)
        {
            Debug.Log("Calling Supportive Contact: " + currentInput);
            StartCoroutine(StartSupportiveCall());
        }
        else
        {
            Debug.Log("Wrong number dialed: " + currentInput);
            StartCoroutine(WrongNumber());
        }
    }

    // ============================================================
    // WRONG NUMBER FEEDBACK
    // ============================================================
    IEnumerator WrongNumber()
    {
        if (audioSource && wrongNumberClip)
            audioSource.PlayOneShot(wrongNumberClip);

        if (displayText != null)
        {
            Vector3 originalPos = displayText.transform.localPosition;

            // shake
            for (int i = 0; i < 10; i++)
            {
                displayText.transform.localPosition =
                    originalPos + new Vector3(Random.Range(-5f, 5f), 0, 0);

                yield return new WaitForSeconds(0.02f);
            }

            displayText.transform.localPosition = originalPos;
            displayText.text = "Number is not reachable.";

            yield return new WaitForSeconds(1.2f);

            currentInput = "";
            displayText.text = "";
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
            currentInput = "";
        }
    }

    // ============================================================
    // SUPPORTIVE CALL SEQUENCE (universal for all personas)
    // ============================================================
    IEnumerator StartSupportiveCall()
    {
        isCalling = true;

        // Ringing
        if (audioSource && connectClip)
        {
            audioSource.PlayOneShot(connectClip);
            yield return new WaitForSeconds(connectClip.length);
        }

        // Show subtitles
        if (subtitlesPanel) subtitlesPanel.SetActive(true);

        // Play dialogue sequence
        for (int i = 0; i < supportiveCallLines.Length; i++)
        {
            var clip = supportiveCallLines[i];
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);

                if (subtitlesText && supportiveSubtitleLines.Length > i)
                    subtitlesText.text = supportiveSubtitleLines[i];

                yield return new WaitForSeconds(clip.length + 0.25f);
            }
        }

        // Hide subtitles
        if (subtitlesPanel) subtitlesPanel.SetActive(false);
        if (subtitlesText) subtitlesText.text = "";

        // Clear number
        currentInput = "";
        if (displayText != null) displayText.text = "";

        // Close phone UI
        PhoneInteraction phone = FindFirstObjectByType<PhoneInteraction>();
        if (phone != null)
            phone.ClosePhone();

        // Trigger ending
        GameEndingManager ending = FindFirstObjectByType<GameEndingManager>();
        if (ending != null)
            ending.StartEndingSequence();
        else
            Debug.LogWarning("GameEndingManager not found in scene.");

        isCalling = false;
    }

    // ============================================================
    // RESET CALL STATE (used when ESC or reopen phone)
    // ============================================================
    public void ResetCallState()
    {
        isCalling = false;
        currentInput = "";

        if (displayText != null)
            displayText.text = "";

        if (subtitlesPanel != null)
            subtitlesPanel.SetActive(false);

        if (subtitlesText != null)
            subtitlesText.text = "";
    }
}
