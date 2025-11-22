using UnityEngine;
using UnityEditor;   // 👈 Needed for Editor curve evaluation
using TMPro;

// 👇 This ensures the script runs even in Edit Mode
[ExecuteAlways]
public class WarpTextExample : MonoBehaviour
{
    public AnimationCurve VertexCurve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(0.25f, 0.5f),
        new Keyframe(0.5f, 1),
        new Keyframe(0.75f, 0.5f),
        new Keyframe(1, 0)
    );

    public float CurveScale = 2.5f;

    private TMP_Text m_TextComponent;

    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // Only run if the text component exists and has text
        if (m_TextComponent == null || !m_TextComponent.havePropertiesChanged)
            return;

        WarpText();
    }

    void WarpText()
    {
        m_TextComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        int characterCount = textInfo.characterCount;

        if (characterCount == 0)
            return;

        float boundsMinX = m_TextComponent.bounds.min.x;
        float boundsMaxX = m_TextComponent.bounds.max.x;

        for (int i = 0; i < characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Calculate the midpoint of each character
            Vector3 charMidBaseline = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2;
            Vector3 offsetToMidBaseline = charMidBaseline;

            // Translate to origin
            vertices[vertexIndex + 0] -= offsetToMidBaseline;
            vertices[vertexIndex + 1] -= offsetToMidBaseline;
            vertices[vertexIndex + 2] -= offsetToMidBaseline;
            vertices[vertexIndex + 3] -= offsetToMidBaseline;

            float x0 = (charMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX);
            float y0 = VertexCurve.Evaluate(x0) * CurveScale;

            // Apply vertical offset (warp)
            Vector3 offset = new Vector3(0, y0, 0);

            vertices[vertexIndex + 0] += offset + offsetToMidBaseline;
            vertices[vertexIndex + 1] += offset + offsetToMidBaseline;
            vertices[vertexIndex + 2] += offset + offsetToMidBaseline;
            vertices[vertexIndex + 3] += offset + offsetToMidBaseline;
        }

        // Push updated vertex data back to the text mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            m_TextComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
