using UnityEngine;

public class Room4Letter : MonoBehaviour
{
    [Header("Letter Content")]
    public string senderName;
    [TextArea(4, 8)] public string bodyText;
    public string codeLine = "Code: 0000";  // what appears at bottom

    [Header("Flags")]
    public bool isSupportiveLetter = false; // true for the “correct” letter

    private void OnMouseDown()
    {
        if (Room4LetterUIManager.Instance == null) return;

        Room4LetterUIManager.Instance.ShowLetter(senderName, bodyText, codeLine);

        Debug.Log($"[Room4] Opened letter from {senderName}. Supportive: {isSupportiveLetter}");
    }
}
