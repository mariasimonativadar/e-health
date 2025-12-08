using UnityEngine;

public class ClueManager : MonoBehaviour
{
    public static ClueManager Instance { get; private set; }

    [Header("Persona clue groups")]
    public GameObject cluesMIA;
    public GameObject cluesMARKO;
    public GameObject cluesALEX;
    public GameObject cluesSARA;

    private void Awake()
    {
        Instance = this;
    }

    // Called by IntroManager after the intro finishes
    public void ActivatePersonaClues()
    {
        string id = GameState.Instance != null
            ? GameState.Instance.selectedPersonaId
            : "MIA";

        if (cluesMIA   != null) cluesMIA.SetActive(id == "MIA");
        if (cluesMARKO != null) cluesMARKO.SetActive(id == "MARKO");
        if (cluesALEX  != null) cluesALEX.SetActive(id == "ALEX");
        if (cluesSARA  != null) cluesSARA.SetActive(id == "SARA");
    }
}
