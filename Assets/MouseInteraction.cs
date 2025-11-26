using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    public Camera mainCamera;
    public float maxDistance = 3f;
    public LayerMask interactableLayer;

    private BottleOutline currentOutline;
    public float highlightDistance = 3f;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Cursor always visible & confined inside game window
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        // Always force cursor settings each frame
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        HandleHighlight();
        HandleClick();
    }

    // -------------------------------------------------------
    //                       CLICK
    // -------------------------------------------------------
    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
            {
                // Prevent outline-only children from blocking click
                BottleOutline outlineOnly = hit.collider.GetComponent<BottleOutline>();
                BottleInspect inspectTarget = hit.collider.GetComponent<BottleInspect>();

                if (outlineOnly != null && inspectTarget == null)
                    return;

                if (hit.transform.CompareTag("Inspectable"))
                {
                    var clue = hit.transform.GetComponent<ClueInteraction>();
                    var bottle = hit.transform.GetComponent<BottleInspect>();

                    if (clue != null) clue.ShowClue();
                    if (bottle != null) bottle.TriggerBottleEvent();
                }
            }
        }
    }

    // -------------------------------------------------------
    //                     HIGHLIGHT
    // -------------------------------------------------------
    void HandleHighlight()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, highlightDistance))
        {
            // Stop highlight if bottle is being inspected
            BottleInspect inspect = hit.collider.GetComponent<BottleInspect>();
            if (inspect != null && inspect.IsInspecting)
            {
                if (currentOutline != null)
                {
                    currentOutline.DisableOutline();
                    currentOutline = null;
                }
                return;
            }

            BottleOutline outline = hit.collider.GetComponent<BottleOutline>();

            if (outline != null)
            {
                outline.EnableOutline();

                if (currentOutline != null && currentOutline != outline)
                    currentOutline.DisableOutline();

                currentOutline = outline;
                return;
            }
        }

        // No hit → remove highlight
        if (currentOutline != null)
        {
            currentOutline.DisableOutline();
            currentOutline = null;
        }
    }
}
