using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class TextCylindricalWrap : MonoBehaviour
{
    [Header("Cylindrical Wrap Settings")]
    [Range(0.01f, 1f)] public float radius = 0.15f;
    public float heightOffset = 0f;
    public float angleOffset = 0f;

    private TextMeshPro tmpText;

    void Awake()
    {
        tmpText = GetComponent<TextMeshPro>();
    }

    void OnValidate()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshPro>();

        UpdateWrap();
    }

    void Update()
    {
#if UNITY_EDITOR
        UpdateWrap();
#endif
    }

    void UpdateWrap()
    {
        if (tmpText == null) return;

        // Force update of the text mesh
        tmpText.ForceMeshUpdate();
        var textInfo = tmpText.textInfo;

        // Iterate through all character meshes
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            var vertices = meshInfo.vertices;

            for (int j = 0; j < textInfo.characterCount; j++)
            {
                var charInfo = textInfo.characterInfo[j];
                if (!charInfo.isVisible) continue;

                int vertexIndex = charInfo.vertexIndex;
                for (int k = 0; k < 4; k++)
                {
                    Vector3 orig = vertices[vertexIndex + k];

                    // ✅ Curve around Z axis (horizontal wrap for lying bottle)
                    float angle = (orig.x / radius) + angleOffset;
                    float x = Mathf.Cos(angle) * radius;
                    float y = orig.y + heightOffset;
                    float z = Mathf.Sin(angle) * radius;

                    vertices[vertexIndex + k] = new Vector3(x, y, z);
                }
            }

            // Apply updated vertices
            meshInfo.mesh.vertices = vertices;
            tmpText.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
