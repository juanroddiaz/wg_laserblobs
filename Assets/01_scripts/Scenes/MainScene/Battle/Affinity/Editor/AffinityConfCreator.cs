using UnityEngine;
using UnityEditor;
using System.IO;

public class AffinityConfCreator : MonoBehaviour
{
    [MenuItem("Tools/Create New Affinity Configuration")]
    private static void CreateNewAffinityConfiguration()
    {
        AffinityConfiguration ac = ScriptableObject.CreateInstance<AffinityConfiguration>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(AffinityConfiguration).ToString() + ".asset");

        AssetDatabase.CreateAsset(ac, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = ac;

        CustomLog.LogWarning("A new instance of AffinityConfiguration was created!");
    }
}
