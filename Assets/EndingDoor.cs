using UnityEngine;
using System.Collections;
using System;

public class EndingDoor : MonoBehaviour
{
    public Transform doorHinge;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    public event Action OnDoorOpened;   // 🔹 ADD THIS

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (doorHinge == null)
        {
            Debug.LogError("❌ EndingDoor: doorHinge is NOT assigned!");
            return;
        }

        closedRotation = doorHinge.localRotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;
    }

    public void OpenDoor()
    {
        if (!isOpen)
            StartCoroutine(OpenRoutine());
    }

    IEnumerator OpenRoutine()
    {
        isOpen = true;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorHinge.localRotation =
                Quaternion.Slerp(closedRotation, openRotation, t);
            yield return null;
        }

        OnDoorOpened?.Invoke(); // 🔹 FIRE EVENT
    }
}
