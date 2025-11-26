using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsMenu : MonoBehaviour
{
    public AudioSource clickSound; // assign in Inspector
    public float delay = 1f;       // 1 second delay before scene change

    public void BackToMenu()
    {
        StartCoroutine(PlaySoundAndLoad());
    }

    private System.Collections.IEnumerator PlaySoundAndLoad()
    {
        // Play the click sound
        if (clickSound != null)
            clickSound.Play();

        // Wait for the sound to finish or delay time
        yield return new WaitForSeconds(delay);

        // Then load the Main Menu
        SceneManager.LoadScene("MainMenu");
    }
}


