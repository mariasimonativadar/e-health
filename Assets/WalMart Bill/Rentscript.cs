using UnityEngine;

public class PickupRent : MonoBehaviour
{
    public GameObject RentImage; // drag the IMAGE inside the Canvas here
    private bool isVisible = false;

    void Start()
    {
        if (RentImage != null)
            RentImage.SetActive(false); // start hidden
    }

    void Update()
    {
        // Hide the UI and bring back the Quad when I is pressed
        if (isVisible && Input.GetKeyDown(KeyCode.I))
        {
            RentImage.SetActive(false);
            gameObject.SetActive(true);
            isVisible = false;
        }
    }

    void OnMouseDown()
    {
        if (!isVisible)
        {
            RentImage.SetActive(true);  // show the Image
            gameObject.SetActive(false);   // hide the Quad
            isVisible = true;
        }
    }
}
