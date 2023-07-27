using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/CompositeCommand", fileName = "SequenceCommandSO")]
    public class CompositeCommandSO : ComplexCommandSO
    {
        public override ITutorialCommand BuildCommand()
        {
            if (commands == null || commands.Length == 0)
            {
                return null;
            }

            ITutorialCommand[] listCommands = new ITutorialCommand[commands.Length];
            for (int i = 0; i < commands.Length; ++i)
            {
                listCommands[i] = commands[i].BuildCommand();
            }

            return new CompositeCommand(listCommands);
        }
    }
}