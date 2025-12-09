using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ClueTracker : MonoBehaviour
{
    [Header("Room Light")]
    public Light roomLight;

    [Header("Light Intensities (Base)")]
    [Tooltip("Light when 0 clues found")]
    public float lightLevel0 = 0.5f;
    [Tooltip("Light after 1st clue")]
    public float lightLevel1 = 2f;
    [Tooltip("Light after 2nd clue")]
    public float lightLevel2 = 4f;
    [Tooltip("Light after 3rd clue (max)")]
    public float lightLevel3 = 7f;

    [Header("Extra Controls")]
    [Tooltip("Multiply all intensities by this value")]
    public float intensityMultiplier = 1f;
    public Color finalLightColor = Color.white;
    public bool changeColorOnFinal = false;

    [Header("Settings")]
    public float lightLerpTime = 1f;

    [Header("Events")]
    public UnityEvent onAllCluesFound;   // Room1Door.UnlockDoor()

    private int cluesFound = 0;

    void Start()
    {
        if (roomLight != null)
        {
            float startIntensity = lightLevel0 * intensityMultiplier;
            roomLight.intensity = startIntensity;
        }
        else
        {
            Debug.LogWarning("ClueTracker: roomLight not assigned!");
        }
    }

    public void RegisterClue()
    {
        cluesFound++;
        Debug.Log("Clue found! Total: " + cluesFound);

        switch (cluesFound)
        {
            case 1:
                SetLightTarget(lightLevel1);
                break;
            case 2:
                SetLightTarget(lightLevel2);
                break;
            case 3:
                SetLightTarget(lightLevel3, true);
                onAllCluesFound?.Invoke();
                break;
        }
    }

    void SetLightTarget(float baseLevel, bool isFinal = false)
    {
        if (roomLight == null) return;

        float target = baseLevel * intensityMultiplier;
        StopAllCoroutines();
        StartCoroutine(LerpRoutine(target, isFinal));
    }

    IEnumerator LerpRoutine(float targetIntensity, bool isFinal)
    {
        float startIntensity = roomLight.intensity;
        Color startColor = roomLight.color;
        Color targetColor = isFinal && changeColorOnFinal ? finalLightColor : startColor;

        float t = 0f;

        while (t < lightLerpTime)
        {
            t += Time.deltaTime;
            float lerp = t / lightLerpTime;

            roomLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, lerp);
            roomLight.color = Color.Lerp(startColor, targetColor, lerp);

            yield return null;
        }

        roomLight.intensity = targetIntensity;
        roomLight.color = targetColor;
    }
}
