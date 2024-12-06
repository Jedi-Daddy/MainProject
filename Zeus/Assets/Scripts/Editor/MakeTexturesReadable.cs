using UnityEditor;
using UnityEngine;

public class MakeTexturesReadable : MonoBehaviour
{
    [MenuItem("Tools/Make Textures Readable")]
    static void SetTexturesReadable()
    {
        // Находим все текстуры в проекте
        string[] texturePaths = AssetDatabase.FindAssets("t:Texture2D");

        foreach (string textureGUID in texturePaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(textureGUID);

            // Получаем импортёр для каждого пути
            AssetImporter importer = AssetImporter.GetAtPath(path);

            // Проверяем, что это TextureImporter
            if (importer is TextureImporter textureImporter)
            {
                textureImporter.isReadable = true; // Делаем текстуру читаемой
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