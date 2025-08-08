using UnityEngine;
using GameDinVR;

/// <summary>
/// Lilybear acts as the voice & operations hub.
/// Receives routed Discord commands and relays them across the bus.
/// </summary>
namespace GameDinVR.Guardians
{
    public class LilybearController : GuardianBase
    {
        [TextArea]
        public string LastMessage;

        private void Start()
        {
            GuardianName = "Lilybear";
            Role = "Voice & Operations";
        }

        public override void OnMessage(string from, string message)
        {
            LastMessage = $"{from}: {message}";

            // Basic routing: "guardian: text" or broadcast with /route
            if (message.StartsWith("/route "))
            {
                var payload = message.Substring(7);
                Whisper("*", payload);
            }
            else
            {
                var idx = message.IndexOf(':');
                if (idx > 0)
                {
                    var target = message.Substring(0, idx).Trim();
                    var content = message.Substring(idx + 1).Trim();
                    Whisper(target, content);
                }
            }
        }

        [ContextMenu("Test Broadcast")]
        private void TestBroadcast()
        {
            Whisper("*", "The council is assembled.");
        }
    }
}
