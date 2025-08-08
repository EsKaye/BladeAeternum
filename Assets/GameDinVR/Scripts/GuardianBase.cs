using UnityEngine;
using GameDinVR;

/// <summary>
/// Base class providing message routing capability for guardians.
/// </summary>
namespace GameDinVR.Guardians
{
    public class GuardianBase : MonoBehaviour
    {
        [Header("Identity")]
        public string GuardianName = "Guardian";
        public string Role = "Undefined";

        protected virtual void OnEnable()
        {
            if (LilybearOpsBus.I != null)
                LilybearOpsBus.I.OnWhisper += HandleWhisper;
        }

        protected virtual void OnDisable()
        {
            if (LilybearOpsBus.I != null)
                LilybearOpsBus.I.OnWhisper -= HandleWhisper;
        }

        protected virtual void HandleWhisper(string from, string to, string message)
        {
            if (to == GuardianName || to == "*")
            {
                Debug.Log($"[{GuardianName}] received from {from}: {message}");
                OnMessage(from, message);
            }
        }

        /// <summary>
        /// Override to react to routed messages.
        /// </summary>
        public virtual void OnMessage(string from, string message) { }

        protected void Whisper(string to, string message)
        {
            LilybearOpsBus.I?.Say(GuardianName, to, message);
        }
    }
}
