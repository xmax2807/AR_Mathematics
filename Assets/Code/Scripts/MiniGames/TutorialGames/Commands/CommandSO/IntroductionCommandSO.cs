using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/IntroCommandSO", fileName = "IntroCommand")]
    public class IntroductionCommandSO : CommandSO
    {
        [SerializeField] private TransformCommand.TransformStruct highLightObjectTransform;
        [SerializeField] private ObjectDataDeliver objectDataDeliver;
        [SerializeField] private string introductionFormat = "%s";
        private const string CharReplace = "%s";
        public override ITutorialCommand BuildCommand()
        {
            SequenceTutorialCommand command = new();
            for (int i = 0; i < objectDataDeliver.Objects.Length; ++i)
            {
                SequenceTutorialCommand subCommand = new();
                TransformCommand transformCommand = new TransformCommand(objectDataDeliver.Objects[i].transform, highLightObjectTransform, 0.5f, isOffset: true);
                subCommand.AddNextCommand(
                    new CompositeCommand(
                        new ITutorialCommand[]{
                            new SpeakCommand(introductionFormat.Replace(CharReplace, objectDataDeliver.Names[i])),
                            transformCommand,
                }));
                subCommand.AddNextCommand(transformCommand.CreateInvertCommand());
                command.AddNextCommand(subCommand);
            }
            return command;
        }
    }
}