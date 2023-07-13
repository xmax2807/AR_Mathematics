using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/SpeakCommand", fileName = "SpeakCommandSO")]
    public class SpeakCommandSO : CommandSO{
        [SerializeField] private string m_text;

        public override ITutorialCommand BuildCommand()
        {
            return new SpeakCommand(m_text);
        }
    }
}