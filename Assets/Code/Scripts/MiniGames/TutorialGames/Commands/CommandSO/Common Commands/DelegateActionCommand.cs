using System.Collections;

namespace Project.MiniGames.TutorialGames
{
    public class DelegateActionCommand : ITutorialCommand
    {
        readonly System.Func<ICommander, IEnumerator> m_action;
        public DelegateActionCommand(System.Func<ICommander, IEnumerator> action){
            m_action = action;
        }

        public IEnumerator Execute(ICommander commander)
        {
            return m_action?.Invoke(commander);
        }
    }
}