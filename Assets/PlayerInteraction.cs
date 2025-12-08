using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;     // How far the player can interact
    public Camera playerCamera;             // Reference to main camera

    void Update()
    {
        // Left mouse click like before
        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("⚠ Player camera not assigned!");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            Debug.Log("🔹 Raycast hit: " + hit.collider.name);

            // 1) LETTERS FIRST
            LetterInteraction letter = hit.collider.GetComponentInParent<LetterInteraction>();
            if (letter != null)
            {
                Debug.Log("📄 Letter found, opening UI");
                letter.OpenLetter();
                return;
            }

            // 2) DOOR SECOND
            DoorInteraction door = hit.collider.GetComponentInParent<DoorInteraction>();
            if (door != null)
            {
                Debug.Log("🚪 Door found, toggling");
                door.ToggleDoor();
                return;
            }

            // 3) BOTTLE THIRD
            BottleInspect bottle = hit.collider.GetComponentInParent<BottleInspect>();
            if (bottle != null)
            {
                Debug.Log("🍾 Bottle found, triggering inspect event");
                bottle.TriggerBottleEvent();
                return;
            }

            // 4) CLUE PANELS (work warning, etc.)
            ClueInteraction clue = hit.collider.GetComponentInParent<ClueInteraction>();
            if (clue != null)
            {
                Debug.Log("🧩 Clue found, showing clue panel");
                clue.ShowClue();
                return;
            }
        }
        else
        {
            Debug.Log("⚫ Nothing hit by raycast");
        }
    }
}
