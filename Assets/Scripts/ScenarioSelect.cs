using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenarioSelectUI : MonoBehaviour
{
    public AudioSource uiAudio;    // optional
    public AudioClip clickSfx;     // optional
    public float delay = 0.25f;
    public GameObject personaModal; // optional: close it first

    public void BackToMenu()
    {
        // Close modal first if it’s open
        if (personaModal != null && personaModal.activeSelf)
        {
            personaModal.SetActive(false);
            return; // press again to go back, or remove this return if you want immediate back after closing
        }

        if (uiAudio && clickSfx) uiAudio.PlayOneShot(clickSfx, 0.8f);
        StartCoroutine(LoadMenuAfterDelay());
    }

    IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu"); // exact scene name
    }
}
