using UnityEditor;
using UnityEngine;

public class URPToStandard
{
    [MenuItem("Tools/Convert URP Materials To Standard")]
    static void Convert()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat == null || mat.shader == null)
                continue;

            string shaderName = mat.shader.name;

            if (shaderName.Contains("Universal Render Pipeline"))
            {
                Texture baseMap = null;
                Color baseColor = Color.white;

                if (mat.HasProperty("_BaseMap"))
                    baseMap = mat.GetTexture("_BaseMap");

                if (mat.HasProperty("_BaseColor"))
                    baseColor = mat.GetColor("_BaseColor");

                mat.shader = Shader.Find("Standard");

                if (baseMap != null)
                    mat.SetTexture("_MainTex", baseMap);

                mat.color = baseColor;

                EditorUtility.SetDirty(mat);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("URP materials converted to Standard");
    }
}
