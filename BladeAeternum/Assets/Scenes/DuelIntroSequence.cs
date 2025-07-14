using UnityEngine;
using System.Collections;

namespace BladeAeternum.Scenes
{
    /// <summary>
    /// Scripts a cinematic duel intro: camera pan, opponent reveal, HUD fade-in, optional lore drop.
    /// Connects to GameManager to trigger before combat starts.
    /// </summary>
    public class DuelIntroSequence : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform panStart;
        [SerializeField] private Transform panEnd;
        [SerializeField] private float panDuration = 2f;
        [SerializeField] private CanvasGroup hudCanvasGroup;
        [SerializeField] private Lore.LoreManager loreManager;
        [SerializeField] private bool playLoreDrop = true;
        #endregion

        #region Unity Methods
        private void Start()
        {
            StartCoroutine(PlayIntroSequence());
        }
        #endregion

        #region Coroutines
        private IEnumerator PlayIntroSequence()
        {
            // Camera pan
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / panDuration;
                mainCamera.transform.position = Vector3.Lerp(panStart.position, panEnd.position, t);
                mainCamera.transform.rotation = Quaternion.Slerp(panStart.rotation, panEnd.rotation, t);
                yield return null;
            }
            // Reveal opponent (could trigger animation here)
            yield return new WaitForSeconds(0.5f);
            // HUD fade-in
            if (hudCanvasGroup != null)
            {
                float fade = 0f;
                while (fade < 1f)
                {
                    fade += Time.deltaTime;
                    hudCanvasGroup.alpha = fade;
                    yield return null;
                }
                hudCanvasGroup.alpha = 1f;
            }
            // Optional lore drop
            if (playLoreDrop && loreManager != null)
            {
                loreManager.ShowPreDuelMessage();
                yield return new WaitForSeconds(2f);
            }
            // Signal GameManager to start combat
            CoreManagers.GameManager.Instance.BeginFight();
        }
        #endregion
    }
} 