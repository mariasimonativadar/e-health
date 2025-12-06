using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Room4LetterUIManager : MonoBehaviour
{
    public static Room4LetterUIManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject fadePanel;      // NEW !!
    public GameObject letterPanel;
    public TMP_Text senderText;
    public TMP_Text bodyText;
    public TMP_Text codeText;
    public Button closeButton;

    [Header("Audio")]
    public AudioSource paperAudio;

    private void Awake()
    {
        Instance = this;

        if (closeButton != null)
            closeButton.onClick.AddListener(HideLetter);

        if (letterPanel != null)
            letterPanel.SetActive(false);

        if (fadePanel != null)
            fadePanel.SetActive(false);   // hide fade at start
    }

    public void ShowLetter(string sender, string body, string codeLine)
    {
        if (senderText != null) senderText.text = sender;
        if (bodyText != null) bodyText.text = body;
        if (codeText != null) codeText.text = codeLine;

        if (fadePanel != null) fadePanel.SetActive(true);   // show fade
        if (letterPanel != null) letterPanel.SetActive(true);

        if (paperAudio != null) paperAudio.Play();
    }

    public void HideLetter()
    {
        if (letterPanel != null) letterPanel.SetActive(false);
        if (fadePanel != null) fadePanel.SetActive(false);   // hide fade
    }

    private void Update()
    {
        if (letterPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            HideLetter();
    }
}
