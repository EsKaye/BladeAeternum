using UnityEngine;

/// <summary>
/// Receives messages from the Discord bot (via MCP /osc or similar)
/// and routes them into the in-scene ops bus so guardians can react.
/// In production this would be triggered by an OSC or WebSocket listener.
/// </summary>
namespace GameDinVR
{
    public class DiscordMessageBridge : MonoBehaviour
    {
        /// <summary>
        /// Entry point called by an external system to relay a message.
        /// </summary>
        public void RouteMessage(string from, string to, string message)
        {
            LilybearOpsBus.I?.Say(from, to, message);
        }

        [ContextMenu("Test Route")]
        private void TestRoute()
        {
            RouteMessage("Discord", "Lilybear", "/route Serafina: bless hall");
        }
    }
}
