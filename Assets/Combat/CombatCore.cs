using UnityEngine;
using System;

namespace BladeAeternum.Combat
{
    /// <summary>
    /// Handles core combat mechanics: slashing, blocking, and timing-based parries.
    /// Modular and event-driven for expansion.
    /// </summary>
    public class CombatCore : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private ParrySystem parrySystem;
        #endregion

        #region Events
        public event Action OnSlash;
        public event Action OnBlock;
        public event Action<bool> OnParryAttempt;
        #endregion

        #region Public Methods
        public void Slash()
        {
            // TODO: Implement slash logic
            Debug.Log("Slash performed!");
            OnSlash?.Invoke();
        }

        public void Block()
        {
            // TODO: Implement block logic
            Debug.Log("Block activated!");
            OnBlock?.Invoke();
        }

        public void TryParry(float inputTime)
        {
            // TODO: Integrate with ParrySystem
            bool success = false;
            if (parrySystem != null)
            {
                success = parrySystem.AttemptParry(inputTime);
            }
            OnParryAttempt?.Invoke(success);
        }
        #endregion
    }
} 