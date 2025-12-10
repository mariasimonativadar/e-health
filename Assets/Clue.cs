using UnityEngine;

public class Clue : MonoBehaviour
{
    [Header("Tracking")]
    public ClueTracker tracker;

    private bool alreadyRegistered = false;

    // ----------------------------------------
    // NORMAL COLLECTION (for clues you pick up)
    // ----------------------------------------
    public void Collect()
    {
        if (!alreadyRegistered)
        {
            RegisterClueOnly();
        }

        // Remove from scene
        Destroy(gameObject);
    }

    // ---------------------------------------------------------
    // REGISTER WITHOUT DESTROYING (for monitor, calendar, etc.)
    // ---------------------------------------------------------
    public void RegisterClueOnly()
    {
        if (alreadyRegistered)
            return;

        alreadyRegistered = true;

        if (tracker != null)
        {
            tracker.RegisterClue();
        }
        else
        {
            Debug.LogWarning("Clue: No ClueTracker assigned on " + name);
        }
    }
}
