using UnityEngine;
using System.Collections;

public class PillClick : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject hallucinationEffect;

    private bool activated = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (activated) return;

        activated = true;
        audioSource.Play();
        StartCoroutine(StartHallucinations());
    }

    private IEnumerator StartHallucinations()
    {
        // Wait 5 seconds before starting hallucinations
        yield return new WaitForSeconds(5f);

        // Activate hallucination effect
        hallucinationEffect.SetActive(true);

        // Wait 6 seconds while effect is active
        yield return new WaitForSeconds(15f);

        // Deactivate hallucination effect
        hallucinationEffect.SetActive(false);
    }
}
