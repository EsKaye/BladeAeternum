using UnityEngine;
using System.Collections.Generic;

namespace BladeAeternum.Characters
{
    /// <summary>
    /// Manages enemy attack patterns for different boss phases. Switches patterns at HP thresholds or triggers.
    /// </summary>
    public class EnemyPatternManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<PatternSet> patternSets;
        [SerializeField] private EnemyAI enemyAI;
        [SerializeField] private float[] hpThresholds;
        #endregion

        #region State
        private int currentPhase = 0;
        #endregion

        #region Unity Methods
        private void Start()
        {
            SwitchPattern(0);
        }
        #endregion

        #region Public Methods
        public void OnHealthChanged(float currentHP, float maxHP)
        {
            for (int i = 0; i < hpThresholds.Length; i++)
            {
                if (currentHP / maxHP <= hpThresholds[i] && currentPhase != i)
                {
                    SwitchPattern(i);
                    break;
                }
            }
        }

        public void SwitchPattern(int phase)
        {
            if (phase < 0 || phase >= patternSets.Count) return;
            currentPhase = phase;
            enemyAI.StopAllCoroutines();
            enemyAI.StartPattern(patternSets[phase]);
        }
        #endregion

        #region PatternSet Definition
        [System.Serializable]
        public class PatternSet
        {
            public string name;
            public List<string> actions; // e.g., "Slash", "Block", "Parry"
            public float actionInterval = 2f;
        }
        #endregion
    }
} 