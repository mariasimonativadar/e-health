using UnityEngine;

public class DoorLock : MonoBehaviour
{
    public MonoBehaviour doorScript; // Drag the original door script here

    void Start()
    {
        // Lock the door by disabling the script
        doorScript.enabled = false;
    }

    public void UnlockDoor()
    {
        // Enable the door script so it can now open
        doorScript.enabled = true;
    }
}
