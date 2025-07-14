using UnityEngine;
using System;

namespace BladeAeternum.Characters
{
    /// <summary>
    /// Simple pattern-based boss combat AI.
    /// </summary>
    public class EnemyAI : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Combat.CombatCore combatCore;
        #endregion

        #region Events
        public event Action OnPatternStart;
        #endregion

        #region Public Methods
        public void StartPattern()
        {
            OnPatternStart?.Invoke();
            StartCoroutine(AttackPattern());
        }
        #endregion

        #region Private Methods
        private System.Collections.IEnumerator AttackPattern()
        {
            while (true)
            {
                // TODO: Implement pattern logic
                combatCore.Slash();
                yield return new WaitForSeconds(2f);
                combatCore.Block();
                yield return new WaitForSeconds(1f);
            }
        }
        #endregion
    }
} 