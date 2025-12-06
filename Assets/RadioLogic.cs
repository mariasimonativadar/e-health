using UnityEngine;

public class RadioLogic : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource supportiveSource;
    public AudioSource[] noiseSources;

    [Header("Settings")]
    [Range(0f, 1f)] public float targetTuning = 0.8f;   // where the "right" voice is
    [Range(0.01f, 0.5f)] public float tolerance = 0.08f; // width around target
    public float fadeSpeed = 2f;

    [Header("Door")]
    public DoorInteractionRoom2 door;

    private float currentTuning = 0.5f;
    private bool doorOpened = false;

    public void SetTuning(float value)
    {
        currentTuning = Mathf.Clamp01(value);
        Debug.Log("Tuning = " + currentTuning);
    }

    private void Update()
    {
        if (supportiveSource == null || noiseSources == null) return;

        float dist = Mathf.Abs(currentTuning - targetTuning);
        float focus = Mathf.Clamp01(1f - dist / tolerance);
        // focus = 0 → far from target, 1 → on target

        float targetSupportiveVol = Mathf.Lerp(0.05f, 1f, focus);
        float targetNoiseVol = Mathf.Lerp(1f, 0f, focus);

        supportiveSource.volume = Mathf.MoveTowards(
            supportiveSource.volume,
            targetSupportiveVol,
            fadeSpeed * Time.deltaTime
        );

        foreach (var n in noiseSources)
        {
            if (n == null) continue;
            n.volume = Mathf.MoveTowards(
                n.volume,
                targetNoiseVol,
                fadeSpeed * Time.deltaTime
            );
        }

        if (!doorOpened && focus > 0.95f)
        {
            doorOpened = true;
            if (door != null)
            {
                door.OpenDoor();
            }
        }
    }
}
