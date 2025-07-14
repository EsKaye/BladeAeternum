using UnityEngine;

namespace BladeAeternum.Combat
{
    /// <summary>
    /// ScriptableObject for blade data: name, model, damage multiplier, unlock requirements.
    /// </summary>
    [CreateAssetMenu(fileName = "BladeData", menuName = "BladeAeternum/Blade Data", order = 1)]
    public class BladeData : ScriptableObject
    {
        #region Blade Properties
        public string bladeName;
        public GameObject bladeModel;
        public float damageMultiplier = 1f;
        [TextArea]
        public string unlockRequirement;
        #endregion
    }
} 