using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Data/PlayerAnswerResultTemplateSO", fileName = "AnswerResultTemplateSO")]
    public class PlayerAnswerResultTemplateSO : CommandSO
    {
        [Header("For Speak command")]
        [SerializeField] private string[] m_speechText;
        [SerializeField] private SpeechAlgorithmType m_algorithmType = SpeechAlgorithmType.Random;

        [Header("For UI command")]
        [SerializeField] private CommandSO uiCommand;

        public override ITutorialCommand BuildCommand()
        {
            CompositeCommand compositeCommand = new(new ITutorialCommand[]{
                new ComplexSpeakCommand(m_speechText, m_algorithmType),
                uiCommand?.BuildCommand(),
            });
            return compositeCommand;
        }
    }
}