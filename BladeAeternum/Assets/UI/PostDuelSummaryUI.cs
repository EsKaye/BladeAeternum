using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BladeAeternum.UI
{
    /// <summary>
    /// UI for post-duel summary: EXP gain, blade unlocks, stat increases, lore obtained. Animates in/out with UnityEvents.
    /// </summary>
    public class PostDuelSummaryUI : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Text expGainText;
        [SerializeField] private Text bladeUnlockText;
        [SerializeField] private Text statIncreaseText;
        [SerializeField] private Text loreText;
        [SerializeField] private CanvasGroup summaryCanvasGroup;
        [SerializeField] private float fadeDuration = 1f;
        [Header("Events")]
        public UnityEvent OnSummaryShown;
        public UnityEvent OnSummaryHidden;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            CoreManagers.GameManager.Instance.OnDuelEnd += OnDuelEnd;
        }
        private void OnDestroy()
        {
            CoreManagers.GameManager.Instance.OnDuelEnd -= OnDuelEnd;
        }
        #endregion

        #region Event Handlers
        private void OnDuelEnd(bool playerWon)
        {
            if (playerWon)
                ShowSummary();
        }
        #endregion

        #region Public Methods
        public void ShowSummary()
        {
            // Populate summary fields (these would be set from game logic)
            expGainText.text = "+100 EXP";
            bladeUnlockText.text = "Unlocked: Aeternum Edge";
            statIncreaseText.text = "+10 Health, +5 Stamina";
            loreText.text = "Lore: The blade remembers every duel.";
            gameObject.SetActive(true);
            StartCoroutine(FadeIn());
            OnSummaryShown?.Invoke();
        }
        public void HideSummary()
        {
            StartCoroutine(FadeOut());
        }
        #endregion

        #region Coroutines
        private System.Collections.IEnumerator FadeIn()
        {
            float t = 0f;
            summaryCanvasGroup.alpha = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                summaryCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
                yield return null;
            }
            summaryCanvasGroup.alpha = 1f;
        }
        private System.Collections.IEnumerator FadeOut()
        {
            float t = 0f;
            summaryCanvasGroup.alpha = 1f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                summaryCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
                yield return null;
            }
            summaryCanvasGroup.alpha = 0f;
            gameObject.SetActive(false);
            OnSummaryHidden?.Invoke();
        }
        #endregion
    }
} 