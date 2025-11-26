using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource; // reference to the audio source
    public AudioClip clickSound;    // the sound to play

    void Start()
    {
        // Get the Button component on this same GameObject
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}

