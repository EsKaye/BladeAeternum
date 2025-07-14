using UnityEngine;
using System;

namespace BladeAeternum.CoreManagers
{
    /// <summary>
    /// Unlocks new blades upon player victory and registers them in BladeInventory.
    /// </summary>
    public class WeaponUnlocker : MonoBehaviour
    {
        #region Events
        public event Action<string> OnBladeUnlocked;
        #endregion

        #region Serialized Fields
        [SerializeField] private Characters.BladeInventory playerInventory;
        #endregion

        #region Public Methods
        public void UnlockBlade(Combat.BladeData bladeData)
        {
            if (playerInventory != null)
            {
                playerInventory.UnlockBlade(bladeData);
                playerInventory.EquipBlade(bladeData);
            }
            Debug.Log($"Blade Unlocked: {bladeData.bladeName}");
            OnBladeUnlocked?.Invoke(bladeData.bladeName);
        }
        #endregion
    }
} 