using UnityEngine;

/// <summary>
/// Simple in-scene event bus allowing guardians to whisper to each other.
/// This lets the Discord bridge hand off messages to specific guardians or broadcast.
/// </summary>
namespace GameDinVR
{
    public class LilybearOpsBus : MonoBehaviour
    {
        public static LilybearOpsBus I;

        private void Awake()
        {
            I = this;
        }

        public delegate void Whisper(string from, string to, string message);
        public event Whisper OnWhisper;

        /// <summary>
        /// Broadcast a message across the council.
        /// </summary>
        public void Say(string from, string to, string message)
        {
            OnWhisper?.Invoke(from, to, message);
            Debug.Log($"[LilybearBus] {from} → {to}: {message}");
        }
    }
}
