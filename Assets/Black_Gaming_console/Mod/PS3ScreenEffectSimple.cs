using UnityEngine;
using System.Collections;

public class PS3WarningEffect : MonoBehaviour
{
    public GameObject redPanel;      // The red fullscreen panel
    public float effectDuration = 2f;

    void OnMouseDown()
    {
        StartCoroutine(ShowWarning());
    }

    IEnumerator ShowWarning()
    {
        if (redPanel != null)
            redPanel.SetActive(true);

        yield return new WaitForSeconds(effectDuration);

        if (redPanel != null)
            redPanel.SetActive(false);
    }
}
