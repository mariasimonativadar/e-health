using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] string scenarioSelectScene = "Profile";

    public AudioSource uiAudio;
    public AudioClip click;
    [Range(0f,1f)] public float volume = 0.7f;
    public float delay = 0.25f;

    public void StartGame()
    {
        if (uiAudio && click) uiAudio.PlayOneShot(click, volume);
        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scenarioSelectScene);
    }
}
