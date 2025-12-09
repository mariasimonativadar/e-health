using UnityEngine;
using System.Collections;

public class Room1Door : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("Lock Settings")]
    public bool startsLocked = true;
    private bool isLocked;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip lockedRattleClip;

    [Header("Room Light")]
    public Light room1Light;
    public float lightFadeTime = 1.5f;

    private Quaternion closedRot;
    private Quaternion openRot;
    private bool isOpen = false;
    private Collider doorCollider;

    void Start()
    {
        closedRot = transform.localRotation;
        openRot = Quaternion.Euler(0f, openAngle, 0f) * closedRot;

        isLocked = startsLocked;

        doorCollider = GetComponent<Collider>();
    }

    void OnMouseDown()
    {
        if (isLocked)
        {
            if (audioSource && lockedRattleClip)
                audioSource.PlayOneShot(lockedRattleClip);

            Debug.Log("Door is locked.");
            return;
        }

        ToggleDoor();
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("ROOM1 DOOR: Unlocked!");
    }

    void ToggleDoor()
    {
        StopAllCoroutines();
        StartCoroutine(DoorRoutine());
    }

    IEnumerator DoorRoutine()
    {
        isOpen = !isOpen;

        Quaternion start = transform.localRotation;
        Quaternion target = isOpen ? openRot : closedRot;

        // Allow walking through when door is open
        if (doorCollider != null)
            doorCollider.isTrigger = isOpen;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            transform.localRotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        // ---- ACTIVATE ROOM 2 AUDIO AFTER DOOR IS FULLY OPEN ----
        if (isOpen)
        {
            Room2AudioActivator zone = FindObjectOfType<Room2AudioActivator>();
            if (zone != null)
            {
                Debug.Log("ROOM1 DOOR: Activating Room2 audio zone.");
                zone.AllowActivation();
            }
        }

        // Light fade
        if (isOpen && room1Light != null)
            StartCoroutine(FadeLightOff());
    }

    IEnumerator FadeLightOff()
    {
        float start = room1Light.intensity;
        float t = 0f;

        while (t < lightFadeTime)
        {
            t += Time.deltaTime;
            room1Light.intensity = Mathf.Lerp(start, 0f, t / lightFadeTime);
            yield return null;
        }

        room1Light.intensity = 0f;
    }
}
