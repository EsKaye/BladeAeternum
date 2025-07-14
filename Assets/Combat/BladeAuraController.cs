using UnityEngine;

namespace BladeAeternum.Combat
{
    /// <summary>
    /// Controls glowing blade aura VFX based on blade type or charged state. Shows aura when blade energy is full.
    /// </summary>
    public class BladeAuraController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private ParticleSystem auraVFX;
        [SerializeField] private BladeData bladeData;
        [SerializeField] private CombatCore combatCore;
        [SerializeField] private float checkInterval = 0.1f;
        #endregion

        #region State
        private float timer = 0f;
        #endregion

        #region Unity Methods
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= checkInterval)
            {
                timer = 0f;
                UpdateAura();
            }
        }
        #endregion

        #region Private Methods
        private void UpdateAura()
        {
            if (combatCore != null && auraVFX != null)
            {
                // Assume CombatCore has a method or property for blade energy (0-1)
                float bladeEnergy = 1f;
                var hud = FindObjectOfType<UI.HUDController>();
                if (hud != null)
                {
                    bladeEnergy = hud.GetComponent<UI.HUDController>().GetComponent<UI.HUDController>().GetType().GetProperty("bladeEnergyBar").GetValue(hud) is UnityEngine.UI.Slider slider ? slider.value : 1f;
                }
                if (bladeEnergy >= 1f)
                {
                    if (!auraVFX.isPlaying) auraVFX.Play();
                }
                else
                {
                    if (auraVFX.isPlaying) auraVFX.Stop();
                }
            }
        }
        #endregion
    }
} 