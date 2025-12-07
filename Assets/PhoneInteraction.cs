using UnityEngine;

public class PhoneInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject phoneCanvas;     // drag PhoneCanvas here
    public Transform player;           // drag PlayerCapsule here

    [Header("Settings")]
    public float interactDistance = 3f;

    private bool phoneOpen = false;

    void Start()
    {
        if (phoneCanvas != null)
            phoneCanvas.SetActive(false);    // hidden at start
    }

    // =====================================
    //  CLICK ON PHONE → OPEN CANVAS
    // =====================================
    void OnMouseDown()
    {
        TogglePhoneCanvas();
    }

    // Called from PlayerInteraction AND from OnMouseDown
    public void TogglePhoneCanvas()
    {
        // Require directory to be read first
        if (!PhoneLogic.directoryUnlocked)
        {
            Debug.Log("Phone is locked. Read the phone directory first.");
            return;
        }

        // Require player to be close enough
        if (player != null &&
            Vector3.Distance(player.position, transform.position) > interactDistance)
        {
            Debug.Log("Too far from phone.");
            return;
        }

        phoneOpen = !phoneOpen;

        if (phoneCanvas != null)
            phoneCanvas.SetActive(phoneOpen);

        if (phoneOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Debug.Log("Phone toggled → " + (phoneOpen ? "OPEN" : "CLOSED"));
    }

    // =====================================
    //  Allow ESC to close the phone
    // =====================================
    void Update()
    {
        if (phoneOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePhone();
        }
    }

    public void ClosePhone()
    {
        phoneOpen = false;

        if (phoneCanvas != null)
            phoneCanvas.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Reset phone logic so the player can dial again
        PhoneLogic logic = FindFirstObjectByType<PhoneLogic>();
        if (logic != null)
            logic.ResetCallState();
    }
}
