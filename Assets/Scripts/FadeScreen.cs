
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public Image fadeImage;
    public float defaultDuration = 1.5f;

    private void Awake()
    {
        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 1f; // start black
            fadeImage.color = c;
        }
    }

    public Coroutine FadeFromBlack(float duration = -1f)
    {
        if (duration <= 0f) duration = defaultDuration;
        return StartCoroutine(Fade(1f, 0f, duration));
    }

    public void SetInstant(float alpha)
    {
        if (fadeImage == null) return;
        var c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (fadeImage == null) yield break;

        float t = 0f;
        var c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}

