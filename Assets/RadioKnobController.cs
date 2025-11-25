using UnityEngine;

public class RadioKnobController : MonoBehaviour
{
    [Header("References")]
    public Transform knobPivot;     // child used as rotation pivot
    public RadioLogic radioLogic;   // script that handles audio

    [Header("Rotation Settings")]
    public float minAngle = -170f;
    public float maxAngle = 170f;
    public float sensitivity = 4f;

    private bool isDragging = false;
    private float currentAngle = 0f;

    private void Start()
    {
        if (knobPivot != null)
        {
            currentAngle = knobPivot.localEulerAngles.x;
            if (currentAngle > 180f)
                currentAngle -= 360f;
        }
    }

    private void Update()
    {
        HandleMouseInput();
        ApplyRotation();
    }

    private void HandleMouseInput()
    {
        // Start dragging when we click on the knob's collider (RadioKnob)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                }
            }
        }

        // Stop drag
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // While dragging, rotate on X axis and send tuning value to RadioLogic
        if (isDragging)
        {
            float mouseDelta = Input.GetAxis("Mouse X") * sensitivity;
            currentAngle += mouseDelta;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

            // Map angle to 0–1
            float normalized = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);

            if (radioLogic != null)
            {
                radioLogic.SetTuning(normalized);
            }
        }
    }

    private void ApplyRotation()
    {
        if (knobPivot != null)
        {
            knobPivot.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);
        }
    }
}
