using UnityEngine;

public class ClueInteraction : MonoBehaviour
{
    public GameObject cluePanel;
    private bool isActive = false;
    private bool hasBeenCounted = false;

    public void ShowClue()
    {
        cluePanel.SetActive(true);
        isActive = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (!hasBeenCounted)
        {
            var tracker = FindObjectOfType<ClueTracker>();
            if (tracker != null)
                tracker.RegisterClue();

            hasBeenCounted = true;
        }
    }

    // 🔥 BUTTON CALLS THIS
    public void CloseCluePanel()
    {
        cluePanel.SetActive(false);
        isActive = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Escape))
            CloseCluePanel();
    }
}
