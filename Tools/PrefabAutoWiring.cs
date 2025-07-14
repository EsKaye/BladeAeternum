#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

/// <summary>
/// PrefabAutoWiring: Scans prefabs/scenes for missing references and auto-assigns components by name/tag.
///
/// URP Compatibility:
/// - All auto-wiring logic is render-pipeline agnostic and works with URP.
/// - Ensure all prefabs and scenes use URP-compatible materials and components.
/// </summary>
public static class PrefabAutoWiring
{
    [MenuItem("BladeAeternum/Auto-Wire Prefabs & Scene References")]
    public static void AutoWireAll()
    {
        // URP: This auto-wiring logic is render-pipeline agnostic. It works for URP, HDRP, and Built-in. Just ensure your prefabs use URP-compatible materials/components.
        // Scan all loaded scenes
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            var scene = EditorSceneManager.GetSceneAt(i);
            foreach (var go in scene.GetRootGameObjects())
                AutoWireGameObject(go);
        }
        // Scan all prefabs in Assets/Prefabs
        var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
        foreach (var guid in prefabGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            AutoWireGameObject(prefab);
        }
        Debug.Log("Auto-wiring complete!");
    }

    private static void AutoWireGameObject(GameObject go)
    {
        if (go == null) return;
        foreach (var comp in go.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (comp == null) continue;
            var fields = comp.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType.IsSubclassOf(typeof(Component)) || field.FieldType == typeof(GameObject))
                {
                    if (field.GetValue(comp) == null)
                    {
                        // Try to find by name
                        var found = go.GetComponentsInChildren(field.FieldType, true).FirstOrDefault(x => x.name.ToLower().Contains(field.Name.ToLower()));
                        if (found != null)
                        {
                            field.SetValue(comp, found);
                            continue;
                        }
                        // Try to find by tag
                        var tagMatch = go.GetComponentsInChildren(field.FieldType, true).FirstOrDefault(x => x.CompareTag(field.Name));
                        if (tagMatch != null)
                        {
                            field.SetValue(comp, tagMatch);
                        }
                    }
                }
            }
        }
    }
}
#endif 