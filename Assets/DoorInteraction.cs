using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorHinge;     // The part of the door that rotates
    public float openAngle = 90f;   // Angle to open to
    public float openSpeed = 2f;    // Speed of opening/closing

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Collider doorCollider;

    void Start()
    {
        // Store the starting rotation of the door
        if (doorHinge == null)
        {
            Debug.LogWarning($"⚠️ Door hinge not assigned on {name}!");
            return;
        }

        closedRotation = doorHinge.localRotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;

        // Cache the collider (on the door hinge or the door mesh)
        doorCollider = doorHinge.GetComponent<Collider>();
        if (doorCollider == null)
        {
            Debug.LogWarning($"⚠️ No collider found on door hinge: {doorHinge.name}");
        }
    }

    public void ToggleDoor()
    {
        if (doorHinge == null)
        {
            Debug.LogError($"❌ Door hinge not set for {name}!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(RotateDoor());
    }

    private IEnumerator RotateDoor()
    {
        isOpen = !isOpen;
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        // Disable collider when open so player can walk through
        if (doorCollider != null)
            doorCollider.isTrigger = isOpen;

        float t = 0f;
        Quaternion startRotation = doorHinge.localRotation;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorHinge.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        Debug.Log($"🚪 Door finished moving: {name} | Open: {isOpen}");
    }
}
