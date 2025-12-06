using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PersonaModal : MonoBehaviour
{
    [Header("UI Refs")]
    public CanvasGroup canvasGroup;       // CanvasGroup on the modal root
    public TextMeshProUGUI nameText;      // Title TMP in the card
    public TextMeshProUGUI bodyText;      // Body TMP in the card

    private PersonaData current;

    void Awake()
    {
        HideInstant();
    }

    public void Open(PersonaData data)
    {
        current = data;
        nameText.text = data.title;
        bodyText.text = data.body;

        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        HideInstant();
    }

    public void StartScenario()
    {
        if (current != null && !string.IsNullOrEmpty(current.startScene))
            SceneManager.LoadScene(current.startScene);
    }

    private void HideInstant()
    {
        gameObject.SetActive(true); // needed to set CG safely
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false); // keep hidden by default
    }
}
