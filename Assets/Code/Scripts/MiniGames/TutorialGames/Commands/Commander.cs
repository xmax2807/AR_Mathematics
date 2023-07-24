using System;
using System.Collections;
using System.Threading.Tasks;
using Project.UI.Panel;
using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public interface ICommander{
        MonoBehaviour GetExecuter();
        void StageEnded();
        void StageRestart();
        void TutorialEnded();
        BasePanelController UIController {get;}
        PracticeTaskUIController PracticeTaskUIController {get;}
        event Action OnStageRestart;
        event Action OnStageEnded;
        event Action OnTutorialEnded;
        void MarkCheckpoint();
        void ReturnToCheckpoint();
        void UpdateCurrentStage(IStage newStage);
    }
    public class Commander : ICommander
    {
        public event Action OnStageEnded;
        public event Action OnStageRestart;
        public event Action OnTutorialEnded;

        private MonoBehaviour m_executer;
        private BasePanelController m_uiController;
        private PracticeTaskUIController m_practiceTaskUIController;

        private IStage _currentStage;
        private int _currentCommandIndex;

        public BasePanelController UIController => m_uiController;
        public PracticeTaskUIController PracticeTaskUIController => m_practiceTaskUIController;

        public Commander(MonoBehaviour executer, BasePanelController uiController, PracticeTaskUIController practiceTaskUIController){
            m_executer = executer;
            m_uiController = uiController;
            m_practiceTaskUIController = practiceTaskUIController;
        }
        public MonoBehaviour GetExecuter()
        {
            return m_executer;
        }

        public void StageEnded()
        {
            OnStageEnded?.Invoke();
        }

        public void StageRestart()
        {
            OnStageRestart?.Invoke();
        }

        public void MarkCheckpoint()
        {
            _currentCommandIndex = _currentStage.CurrentCommandIndex;
        }

        public void UpdateCurrentStage(IStage newStage)
        {
            _currentStage = newStage;
        }

        public void ReturnToCheckpoint()
        {
            _currentStage.MoveToCommand(_currentCommandIndex);
        }

        public void TutorialEnded()
        {
            OnTutorialEnded?.Invoke();
        }
    }
}