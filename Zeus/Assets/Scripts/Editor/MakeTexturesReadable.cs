using UnityEditor;
using UnityEngine;

public class MakeTexturesReadable : MonoBehaviour
{
    [MenuItem("Tools/Make Textures Readable")]
    static void SetTexturesReadable()
    {
        // ������� ��� �������� � �������
        string[] texturePaths = AssetDatabase.FindAssets("t:Texture2D");

        foreach (string textureGUID in texturePaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(textureGUID);

            // �������� ������� ��� ������� ����
            AssetImporter importer = AssetImporter.GetAtPath(path);

            // ���������, ��� ��� TextureImporter
            if (importer is TextureImporter textureImporter)
            {
                textureImporter.isReadable = true; // ������ �������� ��������
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Texture at {path} set to Readable.");
            }
            else
            {
                Debug.LogWarning($"Skipped asset at {path}. It is not a Texture.");
            }
        }
    }
}