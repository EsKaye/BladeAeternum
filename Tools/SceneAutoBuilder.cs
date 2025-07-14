#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// SceneAutoBuilder: One-click setup for BladeAeternum_Intro scene.
/// Creates Player, Enemy, Arena, HUDCanvas, adds all required components, wires references, and saves the scene.
///
/// URP Compatibility:
/// - Adds UniversalAdditionalCameraData to the main camera for URP features (e.g., post-processing, camera stacking).
/// - Adds a global Volume for URP post-processing (user must assign a URP profile in the editor).
/// - All generated objects use URP-compatible components and materials (upgrade materials in Unity if needed).
/// </summary>
public static class SceneAutoBuilder
{
    [MenuItem("BladeAeternum/Auto-Build Intro Scene")]
    public static void BuildIntroScene()
    {
        // Create new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        // Arena Floor
        var arena = GameObject.CreatePrimitive(PrimitiveType.Plane);
        arena.name = "Arena Floor";
        arena.transform.position = Vector3.zero;
        arena.transform.localScale = new Vector3(10, 1, 10);
        // Main Camera
        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        camGO.tag = "MainCamera";
        camGO.transform.position = new Vector3(0, 1, -10);
        camGO.AddComponent<AudioListener>();
        // URP: Add UniversalAdditionalCameraData for URP compatibility
        // This enables URP-specific camera features (post-processing, camera stacking, etc.)
#if UNITY_2021_2_OR_NEWER
        var urpCamDataType = System.Type.GetType("UnityEngine.Rendering.Universal.UniversalAdditionalCameraData, Unity.RenderPipelines.Universal.Runtime");
        if (urpCamDataType != null) camGO.AddComponent(urpCamDataType);
#endif
        // Directional Light
        var lightGO = new GameObject("Directional Light");
        var light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        lightGO.transform.rotation = Quaternion.Euler(50, -30, 0);
        // URP: Add a Volume for post-processing
        // This creates a global Volume for URP post-processing effects (user must assign a URP profile in the editor)
#if UNITY_2021_2_OR_NEWER
        var volumeType = System.Type.GetType("UnityEngine.Rendering.Volume, Unity.RenderPipelines.Core.Runtime");
        if (volumeType != null)
        {
            var volumeGO = new GameObject("Global Volume");
            var volume = volumeGO.AddComponent(volumeType);
            var isGlobalProp = volumeType.GetProperty("isGlobal");
            if (isGlobalProp != null) isGlobalProp.SetValue(volume, true);
            // Optionally, add URP post-processing profile assignment here
        }
#endif
        // Player
        var player = new GameObject("Player");
        player.transform.position = new Vector3(-5, 0, 0);
        player.AddComponent<BladeAeternum.Combat.CombatCore>();
        player.AddComponent<BladeAeternum.Characters.PlayerStats>();
        player.AddComponent<BladeAeternum.Characters.BladeInventory>();
        player.AddComponent<BladeAeternum.Characters.CombatAnimator>();
        player.AddComponent<BladeAeternum.Combat.ParrySystem>();
        // Enemy
        var enemy = new GameObject("Enemy");
        enemy.transform.position = new Vector3(5, 0, 0);
        enemy.AddComponent<BladeAeternum.Characters.EnemyAI>();
        enemy.AddComponent<BladeAeternum.Combat.CombatCore>();
        enemy.AddComponent<BladeAeternum.Characters.EnemyPatternManager>();
        enemy.AddComponent<BladeAeternum.Characters.BladeInventory>();
        enemy.AddComponent<BladeAeternum.Characters.CombatAnimator>();
        // HUDCanvas
        var canvasGO = new GameObject("HUDCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var hud = canvasGO.AddComponent<BladeAeternum.UI.HUDController>();
        // Add sliders
        var healthBar = CreateSlider("HealthBar", canvasGO.transform, Color.red, 0);
        var staminaBar = CreateSlider("StaminaBar", canvasGO.transform, Color.yellow, -40);
        var bladeEnergyBar = CreateSlider("BladeEnergyBar", canvasGO.transform, Color.blue, -80);
        // Assign sliders
        AssignPrivateField(hud, "healthBar", healthBar.GetComponent<Slider>());
        AssignPrivateField(hud, "staminaBar", staminaBar.GetComponent<Slider>());
        AssignPrivateField(hud, "bladeEnergyBar", bladeEnergyBar.GetComponent<Slider>());
        // Managers
        var gm = new GameObject("GameManager").AddComponent<BladeAeternum.CoreManagers.GameManager>();
        var duelFlow = new GameObject("DuelFlowController").AddComponent<BladeAeternum.CoreManagers.DuelFlowController>();
        var outcome = new GameObject("OutcomeSequenceManager").AddComponent<BladeAeternum.CoreManagers.OutcomeSequenceManager>();
        var lore = new GameObject("LoreManager").AddComponent<BladeAeternum.Lore.LoreManager>();
        var unlocker = new GameObject("WeaponUnlocker").AddComponent<BladeAeternum.CoreManagers.WeaponUnlocker>();
        // Wire up DuelFlowController
        AssignPrivateField(duelFlow, "introSequence", null); // User can assign if needed
        AssignPrivateField(duelFlow, "hudCanvasGroup", canvasGO.AddComponent<CanvasGroup>());
        AssignPrivateField(duelFlow, "player", player);
        AssignPrivateField(duelFlow, "enemy", enemy);
        AssignPrivateField(duelFlow, "summaryUI", null); // User can assign if needed
        AssignPrivateField(duelFlow, "outcomeManager", outcome);
        AssignPrivateField(duelFlow, "gameManager", gm);
        // Save scene
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/BladeAeternum_Intro.unity");
        Debug.Log("BladeAeternum_Intro scene auto-built and saved!");
    }

    private static GameObject CreateSlider(string name, Transform parent, Color fillColor, float yOffset)
    {
        var sliderGO = new GameObject(name, typeof(RectTransform), typeof(Slider));
        sliderGO.transform.SetParent(parent);
        var rt = sliderGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 20);
        rt.anchoredPosition = new Vector2(0, yOffset);
        var slider = sliderGO.GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 1;
        // Background
        var bg = new GameObject("Background", typeof(Image));
        bg.transform.SetParent(sliderGO.transform);
        bg.GetComponent<Image>().color = Color.gray;
        slider.targetGraphic = bg.GetComponent<Image>();
        // Fill
        var fill = new GameObject("Fill", typeof(Image));
        fill.transform.SetParent(bg.transform);
        fill.GetComponent<Image>().color = fillColor;
        slider.fillRect = fill.GetComponent<RectTransform>();
        return sliderGO;
    }

    private static void AssignPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null) field.SetValue(obj, value);
    }
}
#endif 