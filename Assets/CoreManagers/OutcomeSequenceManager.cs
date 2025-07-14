using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BladeAeternum.CoreManagers
{
    /// <summary>
    /// Handles cinematic victory and death sequences: slow-mo victory pose, fade-to-black death, UI overlay.
    /// Hooks into GameManager and PlayerStats.
    /// </summary>
    public class OutcomeSequenceManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private CanvasGroup victoryOverlay;
        [SerializeField] private CanvasGroup deathOverlay;
        [SerializeField] private float fadeDuration = 1.5f;
        [SerializeField] private Characters.PlayerStats playerStats;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            GameManager.Instance.OnDuelEnd += OnDuelEnd;
        }
        private void OnDestroy()
        {
            GameManager.Instance.OnDuelEnd -= OnDuelEnd;
        }
        #endregion

        #region Event Handlers
        private void OnDuelEnd(bool playerWon)
        {
            if (playerWon)
                StartCoroutine(PlayVictorySequence());
            else
                StartCoroutine(PlayDeathSequence());
        }
        #endregion

        #region Coroutines
        private IEnumerator PlayVictorySequence()
        {
            Time.timeScale = 0.2f;
            if (victoryOverlay != null)
            {
                float t = 0f;
                while (t < fadeDuration)
                {
                    t += Time.unscaledDeltaTime;
                    victoryOverlay.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                    yield return null;
                }
                victoryOverlay.alpha = 1f;
            }
            Time.timeScale = 1f;
        }
        private IEnumerator PlayDeathSequence()
        {
            if (deathOverlay != null)
            {
                float t = 0f;
                while (t < fadeDuration)
                {
                    t += Time.unscaledDeltaTime;
                    deathOverlay.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                    yield return null;
                }
                deathOverlay.alpha = 1f;
            }
            // Optionally reset or show retry UI
        }
        #endregion
    }
} 