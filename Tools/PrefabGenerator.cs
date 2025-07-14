#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// PrefabGenerator automates creation of basic prefabs for BladeAeternum.
/// Generates HUDCanvas, Enemy, and Arena prefabs in Assets/Prefabs/.
///
/// URP Compatibility:
/// - All generated prefabs use Unity UI and standard GameObjects, which are URP compatible.
/// - Materials should be upgraded to URP via Unity's built-in material upgrader.
/// - No HDRP or legacy render pipeline dependencies.
/// </summary>
public static class PrefabGenerator
{
    [MenuItem("BladeAeternum/Generate Core Prefabs")]
    public static void GeneratePrefabs()
    {
        string prefabPath = "Assets/Prefabs/";
        if (!Directory.Exists(prefabPath))
            Directory.CreateDirectory(prefabPath);

        // Generate HUDCanvas prefab
        GameObject hudCanvas = new GameObject("HUDCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = hudCanvas.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        hudCanvas.AddComponent<BladeAeternum.UI.HUDController>();
        // Add sliders for health, stamina, blade energy
        GameObject healthBar = CreateSlider("HealthBar", hudCanvas.transform, Color.red);
        GameObject staminaBar = CreateSlider("StaminaBar", hudCanvas.transform, Color.yellow);
        GameObject bladeEnergyBar = CreateSlider("BladeEnergyBar", hudCanvas.transform, Color.blue);
        // Assign sliders to HUDController
        var hud = hudCanvas.GetComponent<BladeAeternum.UI.HUDController>();
        hud.GetType().GetField("healthBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(hud, healthBar.GetComponent<Slider>());
        hud.GetType().GetField("staminaBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(hud, staminaBar.GetComponent<Slider>());
        hud.GetType().GetField("bladeEnergyBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(hud, bladeEnergyBar.GetComponent<Slider>());
        // URP: All UI and GameObject components here are URP compatible. If you use custom materials, upgrade them in Unity.
        PrefabUtility.SaveAsPrefabAsset(hudCanvas, prefabPath + "HUDCanvas.prefab");
        Object.DestroyImmediate(hudCanvas);

        // Generate Enemy prefab
        GameObject enemy = new GameObject("Enemy");
        enemy.AddComponent<BladeAeternum.Characters.EnemyAI>();
        enemy.AddComponent<BladeAeternum.Combat.CombatCore>();
        // URP: No render pipeline-specific code. Ensure any custom materials are URP compatible.
        PrefabUtility.SaveAsPrefabAsset(enemy, prefabPath + "Enemy.prefab");
        Object.DestroyImmediate(enemy);

        // Generate Arena prefab
        GameObject arena = GameObject.CreatePrimitive(PrimitiveType.Plane);
        arena.name = "Arena";
        // URP: The default Unity plane uses the Standard shader. Use Unity's material upgrader to convert to URP if needed.
        PrefabUtility.SaveAsPrefabAsset(arena, prefabPath + "Arena.prefab");
        Object.DestroyImmediate(arena);

        Debug.Log("BladeAeternum core prefabs generated in Assets/Prefabs/");
    }

    private static GameObject CreateSlider(string name, Transform parent, Color fillColor)
    {
        GameObject sliderGO = new GameObject(name, typeof(RectTransform), typeof(Slider));
        sliderGO.transform.SetParent(parent);
        Slider slider = sliderGO.GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 1;
        // Add background and fill
        GameObject bg = new GameObject("Background", typeof(Image));
        bg.transform.SetParent(sliderGO.transform);
        bg.GetComponent<Image>().color = Color.gray;
        slider.targetGraphic = bg.GetComponent<Image>();
        GameObject fill = new GameObject("Fill", typeof(Image));
        fill.transform.SetParent(bg.transform);
        fill.GetComponent<Image>().color = fillColor;
        slider.fillRect = fill.GetComponent<RectTransform>();
        return sliderGO;
    }
}
#endif 