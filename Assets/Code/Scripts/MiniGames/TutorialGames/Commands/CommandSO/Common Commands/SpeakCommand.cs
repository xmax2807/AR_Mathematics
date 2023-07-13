using System.Collections;

namespace Project.MiniGames.TutorialGames
{
    public class SpeakCommand : ITutorialCommand
    {
        private string m_text;
        public IEnumerator Execute(ICommander commander)
        {
            Managers.AudioManager.Instance.Speak(m_text);
            yield break;
        }
    }
}