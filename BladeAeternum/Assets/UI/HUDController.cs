using UnityEngine;
using UnityEngine.UI;
using System;

namespace BladeAeternum.UI
{
    /// <summary>
    /// Controls the Heads-Up Display (HUD) for health, stamina, and blade energy.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        [SerializeField] private Slider bladeEnergyBar;
        #endregion

        #region Events
        public event Action<float> OnHealthChanged;
        public event Action<float> OnStaminaChanged;
        public event Action<float> OnBladeEnergyChanged;
        #endregion

        #region Public Methods
        public void UpdateHealth(float value)
        {
            float clamped = Mathf.Clamp01(value);
            healthBar.value = clamped;
            OnHealthChanged?.Invoke(clamped);
        }

        public void UpdateStamina(float value)
        {
            float clamped = Mathf.Clamp01(value);
            staminaBar.value = clamped;
            OnStaminaChanged?.Invoke(clamped);
        }

        public void UpdateBladeEnergy(float value)
        {
            float clamped = Mathf.Clamp01(value);
            bladeEnergyBar.value = clamped;
            OnBladeEnergyChanged?.Invoke(clamped);
        }
        #endregion
    }
} 