using UnityEngine;
using System.Collections.Generic;

namespace BladeAeternum.CoreManagers
{
    /// <summary>
    /// Saves and loads unlocked blades, equipped blade, and player stats using JsonUtility/PlayerPrefs.
    /// </summary>
    public class BladeProgressSave : MonoBehaviour
    {
        #region Save Data Structure
        [System.Serializable]
        public class SaveData
        {
            public List<string> unlockedBladeNames = new List<string>();
            public string equippedBladeName;
            public int level;
            public int experience;
            public int bladeMastery;
            public float maxHealth, maxStamina, maxEnergy;
        }
        #endregion

        #region Public Methods
        public void SaveProgress(Characters.BladeInventory inventory, Characters.PlayerStats stats)
        {
            var data = new SaveData();
            foreach (var blade in inventory.GetUnlockedBlades())
                data.unlockedBladeNames.Add(blade.bladeName);
            var equipped = inventory.GetCurrentBlade();
            data.equippedBladeName = equipped != null ? equipped.bladeName : "";
            data.level = stats.Level;
            data.experience = stats.Experience;
            data.bladeMastery = stats.BladeMastery;
            data.maxHealth = stats.MaxHealth;
            data.maxStamina = stats.MaxStamina;
            data.maxEnergy = stats.MaxEnergy;
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString("BladeProgress", json);
            PlayerPrefs.Save();
        }
        public void LoadProgress(Characters.BladeInventory inventory, Characters.PlayerStats stats, List<Combat.BladeData> allBlades)
        {
            if (!PlayerPrefs.HasKey("BladeProgress")) return;
            string json = PlayerPrefs.GetString("BladeProgress");
            var data = JsonUtility.FromJson<SaveData>(json);
            foreach (var bladeName in data.unlockedBladeNames)
            {
                var blade = allBlades.Find(b => b.bladeName == bladeName);
                if (blade != null) inventory.UnlockBlade(blade);
            }
            var equipped = allBlades.Find(b => b.bladeName == data.equippedBladeName);
            if (equipped != null) inventory.EquipBlade(equipped);
            stats.UpgradeStat("Health", data.maxHealth - stats.MaxHealth);
            stats.UpgradeStat("Stamina", data.maxStamina - stats.MaxStamina);
            stats.UpgradeStat("Energy", data.maxEnergy - stats.MaxEnergy);
            // Set level, experience, mastery
            // (Assume PlayerStats has setters or use reflection if needed)
        }
        #endregion
    }
} 