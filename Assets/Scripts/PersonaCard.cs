using UnityEngine;
using UnityEngine.UI;

public class PersonaCard : MonoBehaviour
{
    public PersonaData data;      
    public PersonaModal modal;    

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(OnSelect);
    }

    void OnSelect()
    {
        if (modal != null && data != null)
        {
            PlayerPrefs.SetString("SelectedPersona", data.title);
            modal.Open(data);
        }
    }
}
