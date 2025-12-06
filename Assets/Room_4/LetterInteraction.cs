using UnityEngine;

public class LetterInteraction : MonoBehaviour
{
    public GameObject letterUIPanel;
    public bool isSupportive = false;
    public string letterCode = "0000";

    public void OpenLetter()
    {
        if (letterUIPanel == null)
        {
            Debug.LogError("❌ No letter UI panel assigned on: " + gameObject.name);
            return;
        }

        letterUIPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("📄 Letter opened: " + gameObject.name);
    }

    public void CloseLetter()
    {
        if (letterUIPanel != null)
            letterUIPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("📄 Letter closed.");
    }
}
