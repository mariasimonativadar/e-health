using UnityEngine;
using System.Collections;

public class DizzyEffect : MonoBehaviour
{
    public Camera cam;

    public void StartDizzy()
    {
        StartCoroutine(DizzyRoutine());
    }

    IEnumerator DizzyRoutine()
    {
        float time = 0f;

        while (time < 12f)  // 5 seconds
        {
            float angle = Mathf.Sin(Time.time * 6f) * 10f; // wobble
            cam.transform.localRotation = Quaternion.Euler(0, 0, angle);

            time += Time.deltaTime;
            yield return null;
        }

        // Reset camera
        cam.transform.localRotation = Quaternion.identity;
    }
}
