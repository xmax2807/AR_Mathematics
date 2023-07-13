using System.Collections;
using System.Collections.Generic;
using Project.Pattern.Command;
namespace Project.MiniGames.TutorialGames{
    public abstract class TutorialWrapperCommand : ITutorialCommand
    {
        private ITutorialCommand m_wrappee;
        private bool m_isChildFirst;
        public TutorialWrapperCommand(ITutorialCommand wrappee, bool isChildFirst = true){
            this.m_wrappee = wrappee;
            this.m_isChildFirst = isChildFirst;
        }
        public IEnumerator Execute(ICommander commander){
            if(m_isChildFirst){
                yield return m_wrappee?.Execute(commander);
                yield return SelfExecute(commander);
            }
            else{
                yield return SelfExecute(commander);
                yield return m_wrappee?.Execute(commander);
            }
        }
        protected virtual IEnumerator SelfExecute(ICommander commander){
            yield break;
        }
    }
    public class SequenceTutorialCommand : ITutorialCommand{

        private List<ITutorialCommand> m_commands;
        
        public SequenceTutorialCommand(){
            m_commands = new();
        }

        public void AddNextCommand(ITutorialCommand command){
            m_commands ??= new();
            m_commands.Add(command);
        }

        public IEnumerator Execute(ICommander commander)
        {
            int count = m_commands.Count;

            for(int i = 0; i < count; ++i){
                yield return m_commands[i].Execute(commander);
            }
        }
    }
}