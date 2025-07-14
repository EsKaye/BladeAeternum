using UnityEngine;
using UnityEngine.UI;

namespace BladeAeternum.UI
{
    /// <summary>
    /// UI panel for displaying player stats and offering upgrade buttons after a duel.
    /// </summary>
    public class UpgradePanelUI : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Characters.PlayerStats playerStats;
        [SerializeField] private Text levelText;
        [SerializeField] private Text healthText;
        [SerializeField] private Text staminaText;
        [SerializeField] private Text energyText;
        [SerializeField] private Button healthUpgradeButton;
        [SerializeField] private Button staminaUpgradeButton;
        [SerializeField] private Button energyUpgradeButton;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            RefreshStats();
            healthUpgradeButton.onClick.AddListener(() => UpgradeStat("Health"));
            staminaUpgradeButton.onClick.AddListener(() => UpgradeStat("Stamina"));
            energyUpgradeButton.onClick.AddListener(() => UpgradeStat("Energy"));
        }
        private void OnDisable()
        {
            healthUpgradeButton.onClick.RemoveAllListeners();
            staminaUpgradeButton.onClick.RemoveAllListeners();
            energyUpgradeButton.onClick.RemoveAllListeners();
        }
        #endregion

        #region Public Methods
        public void RefreshStats()
        {
            levelText.text = $"Level: {playerStats.Level}";
            healthText.text = $"Health: {playerStats.MaxHealth}";
            staminaText.text = $"Stamina: {playerStats.MaxStamina}";
            energyText.text = $"Energy: {playerStats.MaxEnergy}";
        }
        public void UpgradeStat(string stat)
        {
            playerStats.UpgradeStat(stat, 10f); // Example: +10 per upgrade
            RefreshStats();
        }
        #endregion
    }
} 