using UnityEngine;

public class ClueUnlockDoor : MonoBehaviour
{
    public DoorInteraction door;   // drag the DOOR here
    public Transform playerRoot;   // drag PLAYER ROOT here

    private bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.transform.root != playerRoot) return;

        door.UnlockDoor();
        used = true;

        Debug.Log("🧩 Clue collected → door unlocked");
    }
}
