using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSequenceLoader : MonoBehaviour
{
    [Header("Door Reference")]
    public EndingDoor endingDoor;

    [Header("Scene To Load After Door Opens")]
    public string endCreditsSceneName;
    public float delayBeforeCredits = 3f;

    void OnEnable()
    {
        if (endingDoor != null)
            endingDoor.OnDoorOpened += HandleDoorOpened;
    }

    void OnDisable()
    {
        if (endingDoor != null)
            endingDoor.OnDoorOpened -= HandleDoorOpened;
    }

    void HandleDoorOpened()
    {
        StartCoroutine(LoadCredits());
    }

    IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(delayBeforeCredits);

        if (!string.IsNullOrEmpty(endCreditsSceneName))
            SceneManager.LoadScene(endCreditsSceneName);
        else
            Debug.LogError("❌ End Credits scene name not set!");
    }
}
