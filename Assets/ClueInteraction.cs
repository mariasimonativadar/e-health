using UnityEngine;

public class ClueInteraction : MonoBehaviour
{
    public GameObject cluePanel;
    private bool isActive = false;

    public void ShowClue()
    {
        if (!isActive)
        {
            cluePanel.SetActive(true);
            isActive = true;

            // Unlock cursor and show UI
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Escape))
        {
            cluePanel.SetActive(false);
            isActive = false;

            // Return to look mode
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
