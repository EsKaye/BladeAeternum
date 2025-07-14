using UnityEngine;
using System;
using UnityEngine.Events;

namespace BladeAeternum.Combat
{
    /// <summary>
    /// Handles timing-based parry detection, slow-motion, feedback, and triggers UnityEvents for animation/sound.
    /// </summary>
    public class ParrySystem : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private float perfectParryWindow = 0.2f;
        [SerializeField] private float slowMoDuration = 0.5f;
        [SerializeField] private float slowMoTimeScale = 0.2f;
        [SerializeField] private float damageWindow = 0.4f;
        [Header("Feedback")]
        [SerializeField] private AudioClip parrySuccessSFX;
        [SerializeField] private ParticleSystem parryVFX;
        [Header("Events")]
        public UnityEvent OnParrySuccessAnim;
        public UnityEvent OnParrySuccessSFX;
        public UnityEvent OnParryFailAnim;
        #endregion

        #region Events
        public event Action OnParrySuccess;
        public event Action OnParryFail;
        #endregion

        #region State
        public float LastAttackTime { get; set; }
        private bool inDamageWindow = false;
        #endregion

        #region Public Methods
        public bool AttemptParry(float inputTime)
        {
            bool success = Mathf.Abs(inputTime - LastAttackTime) <= perfectParryWindow;
            if (success)
            {
                TriggerParrySuccess();
            }
            else
            {
                TriggerParryFail();
            }
            return success;
        }
        #endregion

        #region Private Methods
        private void TriggerParrySuccess()
        {
            Debug.Log("Perfect Parry!");
            OnParrySuccess?.Invoke();
            OnParrySuccessAnim?.Invoke();
            OnParrySuccessSFX?.Invoke();
            if (parrySuccessSFX)
                AudioSource.PlayClipAtPoint(parrySuccessSFX, transform.position);
            if (parryVFX)
                parryVFX.Play();
            StartCoroutine(SlowMotionCoroutine());
            StartCoroutine(DamageWindowCoroutine());
        }

        private void TriggerParryFail()
        {
            Debug.Log("Parry failed.");
            OnParryFail?.Invoke();
            OnParryFailAnim?.Invoke();
        }

        private System.Collections.IEnumerator SlowMotionCoroutine()
        {
            float originalTimeScale = Time.timeScale;
            Time.timeScale = slowMoTimeScale;
            yield return new WaitForSecondsRealtime(slowMoDuration);
            Time.timeScale = originalTimeScale;
        }

        private System.Collections.IEnumerator DamageWindowCoroutine()
        {
            inDamageWindow = true;
            yield return new WaitForSeconds(damageWindow);
            inDamageWindow = false;
        }
        #endregion

        #region Public Properties
        public bool IsInDamageWindow => inDamageWindow;
        #endregion
    }
} 