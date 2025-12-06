using UnityEngine;

public enum PersonaType
{
    Mia,
    Alex,
    Marko,
    Sara
}

public class Room4PersonaDebug : MonoBehaviour
{
    public static Room4PersonaDebug Instance { get; private set; }

    [Header("Active Persona (for testing)")]
    public PersonaType activePersona = PersonaType.Mia;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Optional: if Room4 is its own scene and you reload, you can keep this
        // DontDestroyOnLoad(gameObject);
    }
}
