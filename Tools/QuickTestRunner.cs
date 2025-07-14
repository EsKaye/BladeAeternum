#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

/// <summary>
/// QuickTestRunner: Editor utility to enter Play Mode, run a test duel, and report results for CI/iteration.
///
/// URP Compatibility:
/// - Test runner logic is render-pipeline agnostic and works with URP.
/// - Ensure test scenes use URP-compatible cameras, lights, and materials.
/// </summary>
public class QuickTestRunner : EditorWindow
{
    [MenuItem("BladeAeternum/Quick Test Duel")] 
    public static void RunTestDuel()
    {
        // URP: This test runner is render-pipeline agnostic. Ensure your test scenes use URP-compatible cameras, lights, and materials for accurate results.
        EditorApplication.EnterPlaymode();
        EditorApplication.update += WaitForDuelEnd;
    }

    private static void WaitForDuelEnd()
    {
        var gm = GameObject.FindObjectOfType<BladeAeternum.CoreManagers.GameManager>();
        if (gm != null && gm.CurrentState == BladeAeternum.CoreManagers.GameManager.DuelState.Outcome)
        {
            Debug.Log($"[QuickTestRunner] Duel ended. State: {gm.CurrentState}");
            EditorApplication.ExitPlaymode();
            EditorApplication.update -= WaitForDuelEnd;
        }
    }
}
#endif 