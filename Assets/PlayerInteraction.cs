using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;     // How far the player can interact
    public Camera playerCamera;             // Reference to main camera

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("⚠️ Player camera not assigned!");
            return;
        }

        // Raycast only on objects in the "Door" layer
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, LayerMask.GetMask("Door")))
        {
            Debug.Log($"🔹 Raycast hit: {hit.collider.name}");

            DoorInteraction door = hit.collider.GetComponentInParent<DoorInteraction>();
            if (door != null)
            {
                Debug.Log($"🚪 Found door: {door.name} | Calling ToggleDoor()");
                door.ToggleDoor();
            }
            else
            {
                Debug.Log("❌ Object hit has no DoorInteraction script");
            }
        }
        else
        {
            Debug.Log("⚫ No door detected within range");
        }
    }
}
