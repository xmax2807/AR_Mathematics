using System.Collections;
using Project.Pattern.Command;
using UnityEngine;

namespace Project.MiniGames.TutorialGames{
    public class CompositeCommand : ITutorialCommand
    {
        private ITutorialCommand[] m_commands;

        public CompositeCommand(ITutorialCommand[] commands){
            this.m_commands = new ITutorialCommand[commands.Length];
            for(int i = 0; i < commands.Length; ++i){
                this.m_commands[i] = commands[i];
            }
        }
        public IEnumerator Execute(ICommander commander)
        {
            if(m_commands == null || m_commands.Length == 0){
                yield break;
            }

            MonoBehaviour executer = commander.GetExecuter();
            Coroutine[] waiters = new Coroutine[m_commands.Length];
            for(int i = 0; i < m_commands.Length; ++i){
                waiters[i] = m_commands[i] == null ? null : executer.StartCoroutine(m_commands[i].Execute(commander));
            }

            for(int i = 0; i < m_commands.Length; ++i){
                yield return waiters[i];
            }
        }   
    }
}