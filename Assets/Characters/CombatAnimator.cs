using UnityEngine;
using System.Collections.Generic;

namespace BladeAeternum.Characters
{
    /// <summary>
    /// Manages combat animations: attack, parry, block, stagger. Supports queuing and blending.
    /// Connects to ParrySystem and CombatCore events.
    /// </summary>
    public class CombatAnimator : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Animator animator;
        [SerializeField] private Combat.CombatCore combatCore;
        [SerializeField] private Combat.ParrySystem parrySystem;
        [Header("Animation States")]
        [SerializeField] private string attackTrigger = "Attack";
        [SerializeField] private string blockTrigger = "Block";
        [SerializeField] private string parryTrigger = "Parry";
        [SerializeField] private string staggerTrigger = "Stagger";
        #endregion

        #region State
        private Queue<string> animationQueue = new Queue<string>();
        private bool isAnimating = false;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (combatCore != null)
            {
                combatCore.OnSlash += QueueAttack;
                combatCore.OnBlock += QueueBlock;
                combatCore.OnParryAttempt += OnParryAttempt;
            }
            if (parrySystem != null)
            {
                parrySystem.OnParrySuccess += QueueParry;
                parrySystem.OnParryFail += QueueStagger;
            }
        }
        private void OnDestroy()
        {
            if (combatCore != null)
            {
                combatCore.OnSlash -= QueueAttack;
                combatCore.OnBlock -= QueueBlock;
                combatCore.OnParryAttempt -= OnParryAttempt;
            }
            if (parrySystem != null)
            {
                parrySystem.OnParrySuccess -= QueueParry;
                parrySystem.OnParryFail -= QueueStagger;
            }
        }
        private void Update()
        {
            if (!isAnimating && animationQueue.Count > 0)
            {
                string nextAnim = animationQueue.Dequeue();
                animator.SetTrigger(nextAnim);
                isAnimating = true;
            }
        }
        #endregion

        #region Animation Queue Methods
        private void QueueAttack() => animationQueue.Enqueue(attackTrigger);
        private void QueueBlock() => animationQueue.Enqueue(blockTrigger);
        private void QueueParry() => animationQueue.Enqueue(parryTrigger);
        private void QueueStagger() => animationQueue.Enqueue(staggerTrigger);
        private void OnParryAttempt(bool success)
        {
            if (success) QueueParry();
            else QueueStagger();
        }
        #endregion

        #region Animation Event Hooks
        // Call this from Animation Event at end of each animation
        public void OnAnimationComplete()
        {
            isAnimating = false;
        }
        #endregion
    }
} 