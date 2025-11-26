using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room2AudioActivator : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource supportiveSource;
    public AudioSource[] noiseSources;

    private bool activated = false;

    private void Start()
    {
        // Make sure this collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        // Stop all audio at the beginning (while player is in Room 1)
        if (supportiveSource != null)
        {
            supportiveSource.Stop();
            supportiveSource.playOnAwake = false;
        }

        foreach (var src in noiseSources)
        {
            if (src == null) continue;
            src.Stop();
            src.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        if (supportiveSource != null)
        {
            supportiveSource.volume = 0f;
            supportiveSource.loop = true;
            supportiveSource.Play();
        }

        foreach (var src in noiseSources)
        {
            if (src == null) continue;
            src.volume = 1f;
            src.loop = true;
            src.Play();
        }

        Debug.Log("Room 2 audio activated (taunts + supportive).");
    }
}
