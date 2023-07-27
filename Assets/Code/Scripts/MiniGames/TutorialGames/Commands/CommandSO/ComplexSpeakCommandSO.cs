using UnityEngine;
namespace Project.MiniGames.TutorialGames{

    [CreateAssetMenu(menuName = "MiniGames/TutorialGames/Commands/ComplexSpeakCommandSO", fileName = "ComplexSpeakCommand")]
    public class ComplexSpeakCommandSO : CommandSO{
        [SerializeField] private string[] m_speechText;
        [SerializeField] private SpeechAlgorithmType m_algorithmType = SpeechAlgorithmType.Random;
        public override ITutorialCommand BuildCommand(){
            return new ComplexSpeakCommand(m_speechText, m_algorithmType);
        }
    }
}