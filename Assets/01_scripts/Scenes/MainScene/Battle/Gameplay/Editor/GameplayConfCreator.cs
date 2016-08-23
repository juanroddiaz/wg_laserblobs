using UnityEngine;
using UnityEditor;
using System.IO;

public class GameplayConfCreator : MonoBehaviour
{
    [MenuItem("Tools/Create New Gameplay Configuration")]
    private static void CreateNewGameplayConfiguration()
    {
        GameplayConfiguration ac = ScriptableObject.CreateInstance<GameplayConfiguration>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(GameplayConfiguration).ToString() + ".asset");

        AssetDatabase.CreateAsset(ac, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = ac;

        CustomLog.LogWarning("A new instance of GameplayConfCreator was created!");
    }
}
