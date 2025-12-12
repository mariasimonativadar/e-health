using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndCreditsAutoLoader : MonoBehaviour
{
    [Header("Next Scene")]
    public string mainMenuSceneName;
    public float delayBeforeLoad = 8f;

    void Start()
    {
        StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(delayBeforeLoad);

        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
        else
            Debug.LogError("❌ Main Menu scene name not set!");
    }
}
