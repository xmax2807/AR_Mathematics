using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public interface ICommander{
        MonoBehaviour GetExecuter();
    }
    public class Commander : ICommander
    {
        private MonoBehaviour m_executer;
        public Commander(MonoBehaviour executer){
            m_executer = executer;
        }
        public MonoBehaviour GetExecuter()
        {
            return m_executer;
        }
    }
}