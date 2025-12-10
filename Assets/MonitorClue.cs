using UnityEngine;

public class MonitorClue : MonoBehaviour
{
    [Header("UI")]
    public GameObject summaryCanvas;
    public Transform player;
    public float interactDistance = 2f;

    [Header("Clue")]
    public Clue clue;   // assign the Clue script on THIS object

    private bool clueRegistered = false;

    void Start()
    {
        if (summaryCanvas != null)
            summaryCanvas.SetActive(false);
    }

    void OnMouseDown()
    {
        // Player must be close
        if (player != null && 
            Vector3.Distance(player.position, transform.position) > interactDistance)
            return;

        // Register clue ONCE
        if (!clueRegistered && clue != null)
        {
            clue.RegisterClueOnly();
            clueRegistered = true;
            Debug.Log("Monitor clue registered");
        }

        // Open UI
        if (summaryCanvas != null)
            summaryCanvas.SetActive(true);

        Time.timeScale = 0f;
    }

    void Update()
    {
        if (summaryCanvas != null &&
            summaryCanvas.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMonitorSummary();
        }
    }

    public void CloseMonitorSummary()
    {
        if (summaryCanvas != null)
            summaryCanvas.SetActive(false);

        Time.timeScale = 1f;
    }
}
