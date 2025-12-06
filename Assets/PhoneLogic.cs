using UnityEngine;
using TMPro;
using System.Collections;

public class PhoneLogic : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text displayText;             
    private string currentInput = "";

    [Header("Dial Settings")]
    public string supportiveNumber = "5551834";   
    public int maxDigits = 7;

    // Set true by PhoneDirectoryBook when directory is opened
    public static bool directoryUnlocked = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip digitBeep;
    public AudioClip deleteBeep;
    public AudioClip connectClip;            
    public AudioClip wrongNumberClip;

    [Header("Mom Call Audio (dialogue sequence)")]
    public AudioClip[] momCallLines;

    [Header("Subtitles for Mom Call")]
    public GameObject subtitlesPanel;
    public TMP_Text subtitlesText;
    public string[] momSubtitleLines;
    public float subtitleLineDuration = 3f;

    private bool isCalling = false;

    void Start()
    {
        currentInput = "";
        if (displayText != null) displayText.text = "";
        if (subtitlesPanel) subtitlesPanel.SetActive(false);

        directoryUnlocked = false;   // reset every time scene loads
    }

    // ============================================================
    //  ADD DIGIT
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
    //  DELETE DIGIT
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
    //  PRESS CALL
    // ============================================================
    public void CallButton()
    {
        if (!directoryUnlocked) return;
        if (isCalling) return;

        if (currentInput == supportiveNumber)
        {
            Debug.Log("Calling MOM: " + currentInput);
            StartCoroutine(StartMomCall());
        }
        else
        {
            Debug.Log("Wrong number dialed: " + currentInput);
            StartCoroutine(WrongNumber());
        }
    }

    // ============================================================
    //  WRONG NUMBER
    // ============================================================
    IEnumerator WrongNumber()
    {
        if (audioSource && wrongNumberClip)
            audioSource.PlayOneShot(wrongNumberClip);

        if (displayText != null)
        {
            Vector3 originalPos = displayText.transform.localPosition;

            for (int i = 0; i < 10; i++)
            {
                displayText.transform.localPosition =
                    originalPos + new Vector3(Random.Range(-5f, 5f), 0, 0);
                yield return new WaitForSeconds(0.02f);
            }

            displayText.transform.localPosition = originalPos;

            displayText.text = "Number unreachable.";
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
    //  MOM CALL SEQUENCE
    // ============================================================
    IEnumerator StartMomCall()
    {
        isCalling = true;

        // 1) Ringing before mom picks up
        if (audioSource && connectClip)
        {
            audioSource.PlayOneShot(connectClip);
            yield return new WaitForSeconds(connectClip.length);
        }

        // 2) Show subtitles panel
        if (subtitlesPanel) subtitlesPanel.SetActive(true);

        // 3) Play mom + Mia dialogue sequence
        for (int i = 0; i < momCallLines.Length; i++)
        {
            var clip = momCallLines[i];
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);

                if (subtitlesText && momSubtitleLines.Length > i)
                    subtitlesText.text = momSubtitleLines[i];

                yield return new WaitForSeconds(clip.length + 0.25f);
            }
        }

        // 4) Hide subtitles, clear display
        if (subtitlesPanel) subtitlesPanel.SetActive(false);
        if (subtitlesText) subtitlesText.text = "";

        currentInput = "";
        if (displayText != null) displayText.text = "";

        // 5) Close the phone UI so the cutscene can take over
        PhoneInteraction phone = Object.FindFirstObjectByType<PhoneInteraction>();
        if (phone != null)
        {
            phone.ClosePhone();
        }

        // 6) TRIGGER ENDING SEQUENCE
        GameEndingManager ending = Object.FindFirstObjectByType<GameEndingManager>();
        if (ending != null)
        {
            ending.StartEndingSequence();
        }
        else
        {
            Debug.LogWarning("GameEndingManager not found in scene.");
        }

        isCalling = false;
    }
}
