using UnityEngine;

namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/CheckpointMarkCommand", fileName = "CheckpointMarkCommand")]
    public class CheckpointMarkCommandSO : CommandSO
    {
        [SerializeField] private CommandSO m_commandAfterCheckpoint;
        public override ITutorialCommand BuildCommand()
        {
            return new CheckpointMarkCommand(m_commandAfterCheckpoint.BuildCommand());
        }
    }
}