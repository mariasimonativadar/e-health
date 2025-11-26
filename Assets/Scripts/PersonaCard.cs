using UnityEngine;
using UnityEngine.UI;

public class PersonaCard : MonoBehaviour
{
    public PersonaData data;    // assign the right persona asset
    public PersonaModal modal;  // drag the single modal here

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (modal != null && data != null)
            modal.Open(data);
    }
}
