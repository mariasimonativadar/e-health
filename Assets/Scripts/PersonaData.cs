using UnityEngine;

[CreateAssetMenu(menuName = "Game/Persona Data", fileName = "NewPersona")]
public class PersonaData : ScriptableObject
{
    public string title;       // e.g., "MIA — Overwhelmed Student"
    [TextArea(6, 20)] public string body;  // the short popup description
    public string startScene;  // scene to load for this persona (e.g., "Scene_Mia")
}
