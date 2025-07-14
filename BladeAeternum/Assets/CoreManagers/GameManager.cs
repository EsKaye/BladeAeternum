using UnityEngine;
using System;

namespace BladeAeternum.CoreManagers
{
    /// <summary>
    /// Manages the overall duel flow: intro, fight, and outcome.
    /// Singleton pattern.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager Instance { get; private set; }
        #endregion

        #region Enums
        public enum DuelState { Intro, Fight, Outcome }
        #endregion

        #region State
        public DuelState CurrentState { get; private set; }
        #endregion

        #region Events
        public event Action OnIntroStart;
        public event Action OnFightStart;
        public event Action<bool> OnDuelEnd;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); }
        }
        #endregion

        #region Public Methods
        public void StartDuel()
        {
            CurrentState = DuelState.Intro;
            Debug.Log("Duel Intro started.");
            OnIntroStart?.Invoke();
        }

        public void BeginFight()
        {
            CurrentState = DuelState.Fight;
            Debug.Log("Fight started.");
            OnFightStart?.Invoke();
        }

        public void EndDuel(bool playerWon)
        {
            CurrentState = DuelState.Outcome;
            Debug.Log(playerWon ? "Player Victory!" : "Player Defeat.");
            OnDuelEnd?.Invoke(playerWon);
        }
        #endregion
    }
} 