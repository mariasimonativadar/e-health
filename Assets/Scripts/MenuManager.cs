using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Scene to open when Start is clicked")]
    [SerializeField] string scenarioSelectScene = "Profile";  // exact scene name

    [Header("Optional: click sound & tiny delay")]
    public AudioSource uiAudio;    // 2D AudioSource
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
