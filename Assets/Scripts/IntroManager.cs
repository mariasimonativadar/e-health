// path: Assets/Scripts/System/IntroManager.cs

using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Header("References")]
    public FadeScreen fadeScreen;
    public SubtitleManager subtitleManager;

    // ← Drag your EXISTING movement script here (e.g., PlayerMovement, FirstPersonController, etc.)
    public MonoBehaviour movementScript;

    public PersonaIntroData personaIntroData;
    public ClueManager clueManager;

    [Header("Timing")]
    public float fadeDuration = 1.5f;

    private bool skipping = false;
    private bool introDone = false;

    private void Start()
    {
        // Disable movement at start of intro
        if (movementScript != null)
            movementScript.enabled = false;

        StartCoroutine(RunIntro());
    }

    private void Update()
    {
        // Skip intro with Space
        if (!introDone && Input.GetKeyDown(KeyCode.Space))
        {
            SkipIntro();
        }
    }

    private IEnumerator RunIntro()
    {
        // Load correct intro based on selected persona
        string persona = GameState.Instance != null
            ? GameState.Instance.selectedPersonaId
            : "MIA";

        PersonaIntro intro = personaIntroData.Get(persona);

        // Fade from black
        if (fadeScreen != null)
            yield return fadeScreen.FadeFromBlack(fadeDuration);

        // Play each subtitle line
        foreach (string line in intro.subtitleLines)
        {
            if (skipping) break;

            subtitleManager.Show(line);
            yield return new WaitForSeconds(intro.lineDuration);
        }

        subtitleManager.Clear();
        FinishIntro();
    }

    private void SkipIntro()
    {
        skipping = true;
        StopAllCoroutines();

        if (fadeScreen != null)
            fadeScreen.SetInstant(0);

        if (subtitleManager != null)
            subtitleManager.Clear();

        FinishIntro();
    }

    private void FinishIntro()
    {
        introDone = true;

        // Re-enable movement
        if (movementScript != null)
            movementScript.enabled = true;

        // Activate clues for the chosen persona
        if (clueManager != null)
            clueManager.ActivatePersonaClues();
    }
}
