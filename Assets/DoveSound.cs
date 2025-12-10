using UnityEngine;

public class DoveSound : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip doveClip;

    [Header("Player (optional)")]
    public Transform player;
    public float interactDistance = 3f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        // Optional: only if player is close
        if (player != null)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist > interactDistance) return;
        }

        if (audioSource != null && doveClip != null)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(doveClip);
        }
        else
        {
            Debug.LogWarning("DoveSound: Missing AudioSource or doveClip on " + name);
        }
    }
}
