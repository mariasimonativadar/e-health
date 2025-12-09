using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Room3Door : MonoBehaviour
{
    [Header("Door Parts")]
    public Transform doorHinge;            // The rotating part of the door
    public float openAngle = -90f;         // Adjust based on your door direction
    public float openSpeed = 2f;           // Speed of door opening animation

    [Header("Next Scene")]
    public string sceneToLoad = "Room4";   // Scene that loads when door fully opens

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Collider doorCollider;

    void Start()
    {
        if (doorHinge == null)
        {
            Debug.LogError($"❌ Room3Door: doorHinge is NOT assigned!");
            return;
        }

        // Save closed rotation and calculate open rotation
        closedRotation = doorHinge.localRotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;

        // Optional: Disable collider so player can walk through when open
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

        // Rotate door smoothly
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorHinge.localRotation = Quaternion.Slerp(startRot, openRotation, t);
            yield return null;
        }

        Debug.Log("🚪 Room 3 Door fully opened — loading Room4...");
        SceneManager.LoadScene(sceneToLoad);
    }
}
