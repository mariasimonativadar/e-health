using UnityEngine;

public class PhoneDirectoryPanel : MonoBehaviour
{
    [Header("Panel Root")]
    public GameObject panelRoot;

    void Awake()
    {
        if (panelRoot == null)
            panelRoot = gameObject;

        panelRoot.SetActive(false);
    }

    void Update()
    {
        // Close with ESC when open
        if (panelRoot.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Open()
    {
        panelRoot.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void Close()
    {
        panelRoot.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }

    // For other scripts to make sure it's hidden without touching time/cursor
    public void CloseImmediate()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);
    }
}
