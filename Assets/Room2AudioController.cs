using UnityEngine;
using System.Collections;

public class Room2AudioController : MonoBehaviour
{
    [Header("Radio Logic")] 
    public RadioLogic radioLogic;       
    public float targetValue = 0.80f;    
    public float tolerance = 0.03f;      

    [Header("Indicator Screen")]
    public Renderer speakerRenderer;    
    public Color wrongColor = Color.red;
    public Color correctColor = Color.green;

    [Header("Door Script")]
    public DoorInteractionRoom2 doorScript;

    [Header("Timing")]
    public float requiredTimeInZone = 25f; // seconds required in green

    private float timer = 0f;
    private bool puzzleSolved = false;

    void Update()
    {
        if (puzzleSolved || radioLogic == null)
            return;

        float tuning = radioLogic.CurrentTuning;
        bool inCorrectZone = Mathf.Abs(tuning - targetValue) <= tolerance;

        // Screen color
        if (speakerRenderer != null)
            speakerRenderer.material.color = inCorrectZone ? correctColor : wrongColor;

        // Debug
        Debug.Log($"[Room2] Tuning={tuning} | InZone={inCorrectZone} | Timer={timer}");

        if (inCorrectZone)
        {
            // Count time staying in green zone
            timer += Time.deltaTime;

            if (timer >= requiredTimeInZone)
            {
                puzzleSolved = true;
                OpenDoor();
            }
        }
        else
        {
            // Reset timer if player leaves the correct zone
            timer = 0f;
        }
    }

    private void OpenDoor()
    {
        Debug.Log("🚪 Opening Room 2 Door after sustained tuning!");

        if (doorScript != null)
        {
            doorScript.OpenDoor();
        }
        else
        {
            Debug.LogError("❌ Door Script NOT assigned!");
        }
    }
}
