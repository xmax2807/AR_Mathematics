using System;
using System.Collections;

namespace Project.MiniGames.TutorialGames{
    public class PlayerPracticeAnswerCommand : ITutorialCommand
    {
        private DelegateActionCommand m_playerAnswerCommand;
        private PracticeTask m_task;
        private int m_answerIndex;
        private ITutorialCommand m_playerCorrectAnswerCommand;
        private ITutorialCommand m_playerFailAnswerCommand;

        public PlayerPracticeAnswerCommand(
            Func<PlayerPracticeAnswerCommand, IEnumerator> playerAnswerDelegate, 
            PracticeTask task,
            ITutorialCommand playerCorrectAnswerCommand,
            ITutorialCommand playerFailAnswerCommand){

            m_playerAnswerCommand = new DelegateActionCommand((commander)=>playerAnswerDelegate?.Invoke(this));
            m_task = task;
            m_playerCorrectAnswerCommand = playerCorrectAnswerCommand;
            m_playerFailAnswerCommand = playerFailAnswerCommand;
            m_answerIndex = -1;
        }
        public IEnumerator Execute(ICommander commander)
        {
            commander.PracticeTaskUIController.SetQuestionText(m_task.GetTaskQuestion());
            commander.PracticeTaskUIController.ShowAsync();

            yield return m_playerAnswerCommand.Execute(commander);

            commander.PracticeTaskUIController.HideAsync();

            if(m_task.IsCorrect(m_answerIndex)){
                yield return m_playerCorrectAnswerCommand?.Execute(commander);
            }
            else{
                yield return m_playerFailAnswerCommand?.Execute(commander);
            }

        }
        public bool SetAnswer(int index){
            m_answerIndex = index;
            return m_task.IsCorrect(m_answerIndex);
        }
    }
}