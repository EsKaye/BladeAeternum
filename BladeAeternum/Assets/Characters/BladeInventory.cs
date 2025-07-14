using UnityEngine;
using System.Collections.Generic;
using System;

namespace BladeAeternum.Characters
{
    /// <summary>
    /// Manages unlocked blades, their stats, and current blade state.
    /// </summary>
    public class BladeInventory : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<Combat.BladeData> unlockedBlades = new List<Combat.BladeData>();
        [SerializeField] private Combat.BladeData currentBlade;
        #endregion

        #region Events
        public event Action<Combat.BladeData> OnBladeEquipped;
        public event Action<Combat.BladeData> OnBladeUnlocked;
        #endregion

        #region Public Methods
        public void UnlockBlade(Combat.BladeData blade)
        {
            if (!unlockedBlades.Contains(blade))
            {
                unlockedBlades.Add(blade);
                OnBladeUnlocked?.Invoke(blade);
            }
        }

        public void EquipBlade(Combat.BladeData blade)
        {
            if (unlockedBlades.Contains(blade))
            {
                currentBlade = blade;
                OnBladeEquipped?.Invoke(blade);
            }
        }

        public bool IsBladeUnlocked(Combat.BladeData blade) => unlockedBlades.Contains(blade);
        public Combat.BladeData GetCurrentBlade() => currentBlade;
        public List<Combat.BladeData> GetUnlockedBlades() => unlockedBlades;
        #endregion
    }
} 