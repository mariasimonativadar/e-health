using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    // This will store the selected persona from your profile screen
    public string selectedPersonaId = "MIA";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
