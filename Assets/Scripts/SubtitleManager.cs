using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public TMP_Text subtitleText;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        Clear();
    }

    public void Show(string text)
    {
        if (!subtitleText || !canvasGroup) return;

        subtitleText.text = text;
        canvasGroup.alpha = 1f;
    }

    public void Clear()
    {
        if (!subtitleText || !canvasGroup) return;

        subtitleText.text = "";
        canvasGroup.alpha = 0f;
    }
}
