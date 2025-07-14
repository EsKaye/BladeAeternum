using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

namespace BladeAeternum.Lore
{
    /// <summary>
    /// Delivers pre-duel lore messages and narrative context. Now supports message queue and typewriter effect.
    /// </summary>
    public class LoreManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private string[] preDuelMessages;
        [SerializeField] private Text messageText;
        [SerializeField] private float typewriterSpeed = 0.04f;
        #endregion

        #region Events
        public event Action<string> OnShowMessage;
        #endregion

        #region State
        private int currentMessage = 0;
        private Queue messageQueue = new Queue();
        private Coroutine typewriterCoroutine;
        #endregion

        #region Public Methods
        public void ShowPreDuelMessage()
        {
            if (preDuelMessages != null && preDuelMessages.Length > 0)
            {
                QueueMessages(preDuelMessages);
                PlayNextMessage();
            }
        }

        public void QueueMessages(string[] messages)
        {
            foreach (var msg in messages)
                messageQueue.Enqueue(msg);
        }

        public void PlayNextMessage()
        {
            if (messageQueue.Count > 0)
            {
                string msg = (string)messageQueue.Dequeue();
                if (typewriterCoroutine != null)
                    StopCoroutine(typewriterCoroutine);
                typewriterCoroutine = StartCoroutine(Typewriter(msg));
                OnShowMessage?.Invoke(msg);
            }
        }
        #endregion

        #region Private Methods
        private IEnumerator Typewriter(string message)
        {
            if (messageText == null)
            {
                Debug.LogWarning("LoreManager: No messageText assigned.");
                yield break;
            }
            messageText.text = "";
            foreach (char c in message)
            {
                messageText.text += c;
                yield return new WaitForSeconds(typewriterSpeed);
            }
        }
        #endregion
    }
} 