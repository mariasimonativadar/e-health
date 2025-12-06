
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Audio (optional)")]
    public AudioSource uiAudio;
    public AudioClip clickSfx;
    [Range(0,1f)] public float clickVol = 0.25f;
    public float delay = 0.25f;

    public void StartGame()
    {
        PlayClick();
        StartCoroutine(LoadAfterDelay("Profile"));
    }

    public void OpenInstructions()
    {
        PlayClick();
        StartCoroutine(LoadAfterDelay("Instructions"));
    }

    public void QuitGame()
    {
        PlayClick();
        Application.Quit();
    }

    void PlayClick(){ if (uiAudio && clickSfx) uiAudio.PlayOneShot(clickSfx, clickVol); }
    IEnumerator LoadAfterDelay(string scene)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(scene);
    }
}
