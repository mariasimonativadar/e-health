using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room2AudioActivator : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource supportiveSource;
    public AudioSource[] noiseSources;

    private bool activated = false;
    private bool canActivate = false;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        if (supportiveSource != null)
        {
            supportiveSource.Stop();
            supportiveSource.playOnAwake = false;
        }

        foreach (var src in noiseSources)
        {
            if (src != null)
            {
                src.Stop();
                src.playOnAwake = false;
            }
        }
    }

    // Called by Room1Door AFTER the door opens
    public void AllowActivation()
    {
        canActivate = true;
        Debug.Log("ROOM2 AUDIO ZONE: Ready to trigger.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if (!canActivate) return;
        if (!other.CompareTag("Player")) return;

        activated = true;

        // Supportive voice
        if (supportiveSource != null)
        {
            supportiveSource.volume = 0f;
            supportiveSource.loop = true;
            supportiveSource.Play();
        }

        // Taunts / noises
        foreach (var src in noiseSources)
        {
            if (src != null)
            {
                src.volume = 1f;
                src.loop = true;
                src.Play();
            }
        }

        Debug.Log("ROOM2 AUDIO ZONE: Audio activated.");
    }

    // 🔻 NEW: stop audio when player leaves this room
    void OnTriggerExit(Collider other)
    {
        if (!activated) return;
        if (!other.CompareTag("Player")) return;

        StopAudio();
        Debug.Log("ROOM2 AUDIO ZONE: Audio stopped (player left Room 2).");
    }

    // 🔻 NEW: helper so you can also call this from other scripts if needed
    public void StopAudio()
    {
        if (supportiveSource != null)
            supportiveSource.Stop();

        foreach (var src in noiseSources)
        {
            if (src != null)
                src.Stop();
        }

        activated = false;
        canActivate = false; // optional: prevent re-activating when coming back
    }
}
