using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonaSelect : MonoBehaviour
{
    public string characterName;        // Example: "Marko"
    public string gameplayScene = "SampleScene";  // your main game scene

    public void SelectCharacter()
    {
        PlayerPrefs.SetString("SelectedCharacter", characterName);
        PlayerPrefs.Save();

        Debug.Log("Selected character: " + characterName);

        SceneManager.LoadScene(gameplayScene);
    }
}
