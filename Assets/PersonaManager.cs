using UnityEngine;

public class PersonaManager : MonoBehaviour
{
    public static PersonaManager Instance;

    public PersonaData ActivePersona { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPersona(PersonaData data)
    {
        ActivePersona = data;
        Debug.Log("✅ Persona selected: " + data.title);
    }
}
