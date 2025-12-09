using UnityEngine;

public class Clue : MonoBehaviour
{
    [Header("Tracking")]
    public ClueTracker tracker;

    public void Collect()
    {
        if (tracker != null)
        {
            tracker.RegisterClue();
        }
        else
        {
            Debug.LogWarning("Clue: no ClueTracker assigned on " + name);
        }

        // Remove clue from the scene (or disable instead if you prefer)
        Destroy(gameObject);
    }
}
