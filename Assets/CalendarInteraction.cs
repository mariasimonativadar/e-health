using UnityEngine;

public class CalendarInteraction : MonoBehaviour
{
    [Header("UI")]
    public GameObject calendarCanvas;      // LukaCalendarCanvas
    public Transform player;               // Player / camera root
    public float interactDistance = 2.2f;

    private Clue clue;                     // Clue script on this object
    private bool clueRegistered = false;   // make sure we only count once

    void Start()
    {
        // Get the Clue component on the same object (if present)
        clue = GetComponent<Clue>();

        if (calendarCanvas != null)
            calendarCanvas.SetActive(false);
    }

    void OnMouseDown()
    {
        // Require player to be close
        if (player != null &&
            Vector3.Distance(player.position, transform.position) > interactDistance)
            return;

        // ⭐ Register this calendar as a clue ONCE using the Clue's tracker
        if (!clueRegistered && clue != null && clue.tracker != null)
        {
            clue.tracker.RegisterClue();
            clueRegistered = true;
        }

        // Show the calendar UI
        if (calendarCanvas != null)
            calendarCanvas.SetActive(true);

        Time.timeScale = 0f; // pause while reading
    }

    void Update()
    {
        // Press ESC to close calendar
        if (calendarCanvas != null &&
            calendarCanvas.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCalendar();
        }
    }

    public void CloseCalendar()
    {
        if (calendarCanvas != null)
            calendarCanvas.SetActive(false);

        Time.timeScale = 1f;
    }
}
