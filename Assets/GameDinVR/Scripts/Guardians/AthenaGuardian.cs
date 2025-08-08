using UnityEngine;

/// <summary>
/// Represents Athena, responding to status inquiries.
/// </summary>
namespace GameDinVR.Guardians
{
    public class AthenaGuardian : GuardianBase
    {
        private void Start()
        {
            GuardianName = "Athena";
            Role = "Strategy & Intelligence";
        }

        public override void OnMessage(string from, string message)
        {
            if (message.Contains("status"))
            {
                Whisper("Lilybear", "Athena: All systems nominal.");
            }
        }
    }
}
