using UnityEngine;

public class PersonaRoom4Activator : MonoBehaviour
{
    public GameObject MiaLetters;
    public GameObject AlexLetters;
    public GameObject MarkoLetters;
    public GameObject SaraLetters;

    void Start()
    {
        string activePersona = PlayerPrefs.GetString("ACTIVE_PERSONA", "MIA");

        // disable all first
        MiaLetters.SetActive(false);
        AlexLetters.SetActive(false);
        MarkoLetters.SetActive(false);
        SaraLetters.SetActive(false);

        // enable only the chosen one
        switch (activePersona)
        {
            case "MIA": MiaLetters.SetActive(true); break;
            case "ALEX": AlexLetters.SetActive(true); break;
            case "MARKO": MarkoLetters.SetActive(true); break;
            case "SARA": SaraLetters.SetActive(true); break;
        }
    }
}
