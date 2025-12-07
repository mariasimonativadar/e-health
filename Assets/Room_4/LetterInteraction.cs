using UnityEngine;
using TMPro;

public class LetterInteraction : MonoBehaviour
{
    [Header("Letter Settings")]
    [TextArea(3, 8)]
    public string letterText;
    public string letterCode;
    public bool isSupportive = false;

    [Header("UI Reference")]
    public GameObject letterUIPanel;
    public TMP_Text uiLetterText;
    public TMP_Text uiCodeText;

    [Header("Audio")]
    public AudioSource audioSource;   // Audio Source component on the letter or UI
    public AudioClip openSound;       // Paper opening sound
    public AudioClip closeSound;      // Paper closing sound

    private bool isOpen = false;

    public void OpenLetter()
    {
        if (letterUIPanel == null)
        {
            Debug.LogError("❌ Letter UI Panel not assigned!");
            return;
        }

        letterUIPanel.SetActive(true);
        isOpen = true;

        if (uiLetterText != null)
            uiLetterText.text = letterText;

        if (uiCodeText != null)
            uiCodeText.text = $"Code: {letterCode}";

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 🔊 Play open sound
        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);

        Debug.Log($"📄 Letter opened: {gameObject.name}");
    }

    public void CloseLetter()
    {
        if (letterUIPanel != null)
            letterUIPanel.SetActive(false);

        isOpen = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 🔊 Play close sound
        if (audioSource != null && closeSound != null)
            audioSource.PlayOneShot(closeSound);

        Debug.Log("📄 Letter closed.");
    }
}
