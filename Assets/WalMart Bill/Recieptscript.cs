using UnityEngine;

public class PickupReceipt : MonoBehaviour
{
    public GameObject receiptImage; // drag the IMAGE inside the Canvas here
    private bool isVisible = false;

    void Start()
    {
        if (receiptImage != null)
            receiptImage.SetActive(false); // start hidden
    }

    void Update()
    {
        // Hide the UI and bring back the Quad when I is pressed
        if (isVisible && Input.GetKeyDown(KeyCode.I))
        {
            receiptImage.SetActive(false);
            gameObject.SetActive(true);
            isVisible = false;
        }
    }

    void OnMouseDown()
    {
        if (!isVisible)
        {
            receiptImage.SetActive(true);  // show the Image
            gameObject.SetActive(false);   // hide the Quad
            isVisible = true;
        }
    }
}
