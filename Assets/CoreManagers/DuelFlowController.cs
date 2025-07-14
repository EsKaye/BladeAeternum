using UnityEngine;
using System.Collections;

namespace BladeAeternum.CoreManagers
{
    /// <summary>
    /// Central controller for duel flow: Intro → Combat → Outcome → Summary → Reset.
    /// Calls DuelIntroSequence, manages HUD, player/enemy state, and GameManager.
    /// </summary>
    public class DuelFlowController : MonoBehaviour
    {
        #region Duel States
        public enum DuelPhase { Intro, Combat, Outcome, Summary, Reset }
        public DuelPhase CurrentPhase { get; private set; }
        #endregion

        #region Serialized Fields
        [SerializeField] private Scenes.DuelIntroSequence introSequence;
        [SerializeField] private CanvasGroup hudCanvasGroup;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject enemy;
        [SerializeField] private UI.PostDuelSummaryUI summaryUI;
        [SerializeField] private OutcomeSequenceManager outcomeManager;
        [SerializeField] private GameManager gameManager;
        #endregion

        #region Unity Methods
        private void Start()
        {
            StartCoroutine(DuelLoop());
        }
        #endregion

        #region Duel Loop
        private IEnumerator DuelLoop()
        {
            while (true)
            {
                // Intro
                SetPhase(DuelPhase.Intro);
                hudCanvasGroup.alpha = 0f;
                introSequence.gameObject.SetActive(true);
                yield return new WaitUntil(() => gameManager.CurrentState == GameManager.DuelState.Fight);
                introSequence.gameObject.SetActive(false);
                // Combat
                SetPhase(DuelPhase.Combat);
                hudCanvasGroup.alpha = 1f;
                EnablePlayer(true);
                EnableEnemy(true);
                yield return new WaitUntil(() => gameManager.CurrentState == GameManager.DuelState.Outcome);
                // Outcome
                SetPhase(DuelPhase.Outcome);
                EnablePlayer(false);
                EnableEnemy(false);
                outcomeManager.gameObject.SetActive(true);
                yield return new WaitForSeconds(2f);
                outcomeManager.gameObject.SetActive(false);
                // Summary
                SetPhase(DuelPhase.Summary);
                summaryUI.ShowSummary();
                yield return new WaitUntil(() => summaryUI.gameObject.activeSelf == false);
                // Reset
                SetPhase(DuelPhase.Reset);
                ResetDuel();
                yield return null;
            }
        }
        #endregion

        #region Phase Control
        private void SetPhase(DuelPhase phase)
        {
            CurrentPhase = phase;
            Debug.Log($"Duel Phase: {phase}");
        }
        private void EnablePlayer(bool enable)
        {
            if (player != null)
                player.SetActive(enable);
        }
        private void EnableEnemy(bool enable)
        {
            if (enemy != null)
                enemy.SetActive(enable);
        }
        private void ResetDuel()
        {
            // Optionally respawn or reset player/enemy state here
            EnablePlayer(true);
            EnableEnemy(true);
            gameManager.StartDuel();
        }
        #endregion
    }
} 