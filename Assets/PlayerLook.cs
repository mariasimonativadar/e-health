using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 150f;
    public Transform playerBody;

    [HideInInspector] public bool canLook = true;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float startX = transform.localEulerAngles.x;
        if (startX > 180f) startX -= 360f;
        xRotation = Mathf.Clamp(startX, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Update()
    {
        if (!canLook) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical look
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal look
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }

    // Called at the end of intro
    public void SetRotation(float newXAngle)
    {
        if (newXAngle > 180f) newXAngle -= 360f;

        xRotation = Mathf.Clamp(newXAngle, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
