using System.Collections;

namespace Project.MiniGames.TutorialGames
{
    public class SpeakCommand : ITutorialCommand
    {
        private string m_text;
        public SpeakCommand(string text){
            m_text = text;
        }
        public IEnumerator Execute(ICommander commander)
        {
            yield return Managers.AudioManager.Instance.SpeakAndWait(m_text);
        }
    }
}