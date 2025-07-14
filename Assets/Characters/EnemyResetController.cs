using UnityEngine;

namespace BladeAeternum.Characters
{
    /// <summary>
    /// Handles enemy reset/respawn after duel. Rebinds AI, pattern, and blade data. Positions enemy in arena.
    /// </summary>
    public class EnemyResetController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private EnemyAI enemyAI;
        [SerializeField] private EnemyPatternManager patternManager;
        [SerializeField] private Combat.BladeData[] possibleBlades;
        [SerializeField] private Transform spawnPoint;
        #endregion

        #region Public Methods
        public void ResetEnemy()
        {
            // Optionally randomize blade and pattern
            if (possibleBlades.Length > 0)
            {
                var blade = possibleBlades[Random.Range(0, possibleBlades.Length)];
                enemyAI.GetComponent<BladeInventory>()?.EquipBlade(blade);
            }
            patternManager.SwitchPattern(Random.Range(0, patternManager.PatternSetCount));
            // Reset position
            if (spawnPoint != null)
                enemyAI.transform.position = spawnPoint.position;
            // Optionally reset health, state, etc.
            enemyAI.gameObject.SetActive(true);
        }
        #endregion
    }
} 