using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform doorHinge;           // The part of the door that rotates
    public float openAngle = 90f;         // Angle to open to
    public float openSpeed = 2f;          // Speed of opening/closing

    [Header("Lock Settings")]
    public bool isLocked = true;          // Start locked for Room3 final door

    [Header("Scene Loading (optional)")]
    public bool loadSceneOnOpen = false;  // Tick ONLY on Room3 final door
    public string sceneToLoad = "Room4";  // Next scene name

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Collider doorCollider;

    void Start()
    {
        if (doorHinge == null)
        {
            Debug.LogWarning($"⚠️ Door hinge not assigned on {name}!");
            return;
        }

        closedRotation = doorHinge.localRotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;

        doorCollider = doorHinge.GetComponent<Collider>();
        if (doorCollider == null)
        {
            Debug.LogWarning($"⚠️ No collider found on door hinge: {doorHinge.name}");
        }
    }

    // ✅ Makes door clickable even without a separate interaction manager
    void OnMouseDown()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        if (isLocked)
        {
            Debug.Log("🚫 Door is locked: " + name);
            return;
        }

        if (doorHinge == null)
        {
            Debug.LogError($"❌ Door hinge not set for {name}!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(RotateDoor());
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("🔓 Door unlocked (player must still click to open): " + name);
    }

    private IEnumerator RotateDoor()
    {
        isOpen = !isOpen;
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

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

        if (isOpen && loadSceneOnOpen && !string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.Log($"➡️ Loading scene: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
