using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    public Light lampLight;          // Assign your LampLight here
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.3f;
    public float flickerSpeed = 0.5f;  // Adjust for slower/faster flicker
    public float smoothness = 0.3f;  // Lower = faster jumps, higher = smoother

    private float targetIntensity;
    private float currentVelocity;

    void Start()
    {
        if (lampLight == null)
            lampLight = GetComponent<Light>();
        targetIntensity = lampLight.intensity;
    }

    void Update()
    {
        // Randomly pick a new intensity target over time
        if (Random.value < Time.deltaTime * flickerSpeed)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        // Smooth transition between intensities
        lampLight.intensity = Mathf.SmoothDamp(
            lampLight.intensity,
            targetIntensity,
            ref currentVelocity,
            smoothness
        );
    }
}
