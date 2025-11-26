using UnityEngine;

public class BottleOutline : MonoBehaviour
{
    private Renderer rend;
    private Material[] originalMaterials;
    public Material outlineMaterial;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMaterials = rend.materials;
    }

    public void EnableOutline()
    {
        if (outlineMaterial == null)
        {
            Debug.LogError("Missing outline material!", this);
            return;
        }

        rend.materials = new Material[] { outlineMaterial };
    }

    public void DisableOutline()
    {
        rend.materials = originalMaterials;
    }
}
