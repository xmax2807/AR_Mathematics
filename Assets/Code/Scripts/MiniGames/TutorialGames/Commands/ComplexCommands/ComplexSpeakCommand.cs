using System.Collections;

namespace Project.MiniGames.TutorialGames{
    public enum SpeechAlgorithmType{
        Random, Increasemental,
    }
    public interface ISpeechPickingAlgorithm{
        string PickASpeech(string[] speeches);
    }
    public class RandomSpeechPicking : ISpeechPickingAlgorithm{
        public string PickASpeech(string[] speeches){
            int index = UnityEngine.Random.Range(0, speeches.Length);
            return speeches[index];
        }
    }

    public class ComplexSpeakCommand : ITutorialCommand
    {
        private ISpeechPickingAlgorithm m_algorithm;
        private string[] m_texts;
        public ComplexSpeakCommand(string[] texts) : this(texts,new RandomSpeechPicking()){}
        public ComplexSpeakCommand(string[] texts, SpeechAlgorithmType type) : this(texts){
            switch(type){
                //case SpeechAlgorithmType.Increasemental: break;
                default:
                    m_algorithm = new RandomSpeechPicking(); 
                break;
            }
        }
        public ComplexSpeakCommand(string[] texts, ISpeechPickingAlgorithm algorithm)
        {
            m_algorithm = algorithm;
            m_texts = texts;
        }

        public IEnumerator Execute(ICommander commander)
        {
            SpeakCommand realCommand = new(m_algorithm.PickASpeech(m_texts));
            yield return realCommand.Execute(commander);
        }
    }
}