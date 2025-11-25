using UnityEngine;
using System.Collections;

public class DoorInteractionRoom2 : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorHinge;
    public float openAngle = -90f;
    public float openSpeed = 1f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Collider doorCollider;

    private void Start()
    {
        if (doorHinge == null)
            doorHinge = transform;

        closedRotation = doorHinge.localRotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;

        doorCollider = doorHinge.GetComponent<Collider>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        StopAllCoroutines();
        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        float t = 0f;
        Quaternion startRot = doorHinge.localRotation;

        if (doorCollider != null)
            doorCollider.isTrigger = true;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorHinge.localRotation = Quaternion.Slerp(startRot, openRotation, t);
            yield return null;
        }

        Debug.Log("Room 2 door fully opened.");
    }
}
