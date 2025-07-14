using UnityEngine;
using System;

namespace BladeAeternum.Characters
{
    /// <summary>
    /// Handles player stats: level, experience, blade mastery, health, stamina, and energy. Supports upgrades post-duel.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private int level = 1;
        [SerializeField] private int experience = 0;
        [SerializeField] private int experienceToNextLevel = 100;
        [SerializeField] private int bladeMastery = 0;
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxStamina = 50f;
        [SerializeField] private float maxEnergy = 30f;
        #endregion

        #region State
        private float currentHealth;
        private float currentStamina;
        private float currentEnergy;
        #endregion

        #region Events
        public event Action<int> OnLevelUp;
        public event Action<int> OnExperienceChanged;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnStaminaChanged;
        public event Action<float> OnEnergyChanged;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentEnergy = maxEnergy;
        }
        #endregion

        #region Public Methods
        public void AddExperience(int amount)
        {
            experience += amount;
            OnExperienceChanged?.Invoke(experience);
            if (experience >= experienceToNextLevel)
                LevelUp();
        }

        public void LevelUp()
        {
            level++;
            experience = 0;
            experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f);
            OnLevelUp?.Invoke(level);
        }

        public void UpgradeStat(string stat, float amount)
        {
            switch (stat)
            {
                case "Health": maxHealth += amount; break;
                case "Stamina": maxStamina += amount; break;
                case "Energy": maxEnergy += amount; break;
                case "BladeMastery": bladeMastery += Mathf.RoundToInt(amount); break;
            }
        }

        public void SetHealth(float value)
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }
        public void SetStamina(float value)
        {
            currentStamina = Mathf.Clamp(value, 0, maxStamina);
            OnStaminaChanged?.Invoke(currentStamina);
        }
        public void SetEnergy(float value)
        {
            currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
            OnEnergyChanged?.Invoke(currentEnergy);
        }
        #endregion

        #region Public Properties
        public int Level => level;
        public int Experience => experience;
        public int BladeMastery => bladeMastery;
        public float MaxHealth => maxHealth;
        public float MaxStamina => maxStamina;
        public float MaxEnergy => maxEnergy;
        public float CurrentHealth => currentHealth;
        public float CurrentStamina => currentStamina;
        public float CurrentEnergy => currentEnergy;
        #endregion
    }
} 