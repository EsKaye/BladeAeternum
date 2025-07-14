using UnityEngine;
using UnityEngine.Events;

namespace BladeAeternum.Lore
{
    /// <summary>
    /// Triggers lore drops via interaction or proximity. Fires UnityEvent for voice lines/animations.
    /// </summary>
    public class LoreTrigger : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private string loreMessage;
        [SerializeField] private bool triggerOnProximity = true;
        [SerializeField] private float triggerRadius = 3f;
        [Header("Events")]
        public UnityEvent OnLoreTriggered;
        #endregion

        #region State
        private bool triggered = false;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (triggerOnProximity && !triggered)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, triggerRadius);
                foreach (var hit in hits)
                {
                    if (hit.CompareTag("Player"))
                    {
                        TriggerLore();
                        break;
                    }
                }
            }
        }
        #endregion

        #region Public Methods
        public void TriggerLore()
        {
            if (triggered) return;
            triggered = true;
            Debug.Log($"Lore Triggered: {loreMessage}");
            OnLoreTriggered?.Invoke();
        }
        #endregion
    }
} 