using UnityEngine;

public class LukaCalendarInteraction : MonoBehaviour
{
    public GameObject calendarCanvas;
    public Transform player;
    public float interactDistance = 2f;

    public void OnMouseDown()
    {
        if (Vector3.Distance(player.position, transform.position) > interactDistance)
            return;

        // Register clue
        Clue clue = GetComponent<Clue>();
        if (clue != null)
            clue.Collect();

        calendarCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (calendarCanvas != null && calendarCanvas.activeSelf)
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseCalendar();
    }

    public void CloseCalendar()
    {
        calendarCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
