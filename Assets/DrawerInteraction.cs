using UnityEngine;
using System.Collections;

public class DrawerInteraction : MonoBehaviour
{
    [Header("Moving Drawer Part")]
    public Transform drawer;            // The part that slides

    [Header("Player")]
    public Transform player;            // Assign Player
    public float interactDistance = 3f;

    [Header("Movement Settings")]
    public Vector3 openOffset = new Vector3(0.3f, 0f, 0f); // Drawer moves on X by default
    public float speed = 4f;

    private bool isOpen = false;
    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        if (drawer == null)
        {
            Debug.LogError("DrawerInteraction: drawer not assigned!");
            return;
        }

        // Save both positions
        closedPos = drawer.localPosition;
        openPos = closedPos + openOffset;
    }

    void OnMouseDown()
    {
        // Require player near
        if (player != null)
        {
            float dist = Vector3.Distance(player.position, transform.position);
            if (dist > interactDistance) return;
        }

        // Toggle movement
        StopAllCoroutines();
        StartCoroutine(MoveDrawer());
    }

    IEnumerator MoveDrawer()
    {
        Vector3 start = drawer.localPosition;
        Vector3 target = isOpen ? closedPos : openPos;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            drawer.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }

        drawer.localPosition = target;
        isOpen = !isOpen;
    }
}
