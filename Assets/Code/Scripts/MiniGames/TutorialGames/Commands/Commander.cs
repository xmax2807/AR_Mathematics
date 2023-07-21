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
        BasePanelController UIController {get;}
        event Action OnStageRestart;
        event Action OnStageEnded;
        void MarkCheckpoint();
        void ReturnToCheckpoint();
        void UpdateCurrentStage(IStage newStage);
    }
    public class Commander : ICommander
    {
        public event Action OnStageEnded;
        public event Action OnStageRestart;

        private MonoBehaviour m_executer;
        private BasePanelController m_uiController;

        private IStage _currentStage;
        private int _currentCommandIndex;

        public BasePanelController UIController => m_uiController;
        public Commander(MonoBehaviour executer, BasePanelController uiController){
            m_executer = executer;
            m_uiController = uiController;
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
    }
}