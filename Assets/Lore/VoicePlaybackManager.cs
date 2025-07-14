using UnityEngine;
using System.Collections.Generic;

namespace BladeAeternum.Lore
{
    /// <summary>
    /// Manages voice audio clips tied to dialogue lines. Falls back to text if no clip assigned.
    /// </summary>
    public class VoicePlaybackManager : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<DialogueVoiceClip> voiceClips;
        #endregion

        #region Public Methods
        public void PlayVoice(string dialogueKey, string fallbackText)
        {
            var clip = voiceClips.Find(vc => vc.dialogueKey == dialogueKey);
            if (clip != null && clip.audioClip != null)
            {
                audioSource.clip = clip.audioClip;
                audioSource.Play();
            }
            else
            {
                Debug.Log($"[VoicePlayback] No voice for '{dialogueKey}', fallback: {fallbackText}");
            }
        }
        #endregion

        #region DialogueVoiceClip Definition
        [System.Serializable]
        public class DialogueVoiceClip
        {
            public string dialogueKey; // e.g., "scene_intro", "player_ready"
            public AudioClip audioClip;
        }
        #endregion
    }
} 