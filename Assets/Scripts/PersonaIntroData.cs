// path: Assets/Scripts/System/PersonaIntroData.cs

using UnityEngine;

[System.Serializable]
public class PersonaIntro
{
    public string personaId;
    [TextArea] public string[] subtitleLines;
    public float lineDuration = 3f;
}

public class PersonaIntroData : MonoBehaviour
{
    public PersonaIntro[] intros;

    public PersonaIntro Get(string id)
    {
        foreach (var intro in intros)
        {
            if (intro.personaId == id)
                return intro;
        }

        return intros.Length > 0 ? intros[0] : null;
    }
}
