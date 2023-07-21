using System.Collections;

namespace Project.MiniGames.TutorialGames{
    public class CheckpointMarkCommand : ITutorialCommand
    {
        private bool markedCheckpoint;
        private ITutorialCommand m_commandAfterCheckpoint;

        public CheckpointMarkCommand(ITutorialCommand commandAfterCheckpoint){
            this.markedCheckpoint = false;
            this.m_commandAfterCheckpoint = commandAfterCheckpoint;
        }
        public IEnumerator Execute(ICommander commander)
        {
            if(!markedCheckpoint){
                commander.MarkCheckpoint();
                markedCheckpoint = true;
                return null;
            }
            return m_commandAfterCheckpoint.Execute(commander);
        }
    }
}