using UnityEngine;

public class Room3CorrectClue : MonoBehaviour
{
    public DoorInteraction door;
    public Transform player;
    public float interactDistance = 2f;

    private bool used = false;

    void OnMouseDown()
    {
        if (used) return;

        if (player != null &&
            Vector3.Distance(player.position, transform.position) > interactDistance)
            return;

        if (door == null)
        {
            Debug.LogError("❌ Room3CorrectClue: Door reference not assigned on " + name);
            return;
        }

        door.UnlockDoor();
        used = true;

        Debug.Log("✅ Correct object touched → Door unlocked");
    }
}
