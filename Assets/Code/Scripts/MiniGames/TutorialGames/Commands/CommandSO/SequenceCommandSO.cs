using UnityEngine;

namespace Project.MiniGames.TutorialGames{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/SequenceCommand", fileName = "SequenceCommandSO")]
    public class SequenceCommandSO : ComplexCommandSO
    {
        public override ITutorialCommand BuildCommand()
        {
            if(commands == null || commands.Length == 0){
                return null;
            }
            SequenceTutorialCommand sequenceTutorialCommand = new();

            for(int i = 0; i < commands.Length; ++i){
                sequenceTutorialCommand.AddNextCommand(commands[i].BuildCommand());
            }

            return sequenceTutorialCommand;
        }
    }
}