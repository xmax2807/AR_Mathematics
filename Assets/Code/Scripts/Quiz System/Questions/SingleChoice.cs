using UnityEngine.UI;
using Project.Managers;

namespace Project.QuizSystem{
    public interface ISingleChoice 
    {   
        string[] GetOptions();
        int AnswerIndex {get;}
    }
    public abstract class SingleChoice<T> : BaseQuestion<int>, ISingleChoice
    {
        public override QuestionType QuestionType => QuestionType.SingleChoice;
        protected T[] options;
        protected string[] cacheOptions;
        public int AnswerIndex => _answer;
        public SingleChoice(string question,T[] options, int answer) : base(question, answer)
        {
            _playerAnswered = -1;
            this.options = options;
        }
        public override bool HasAnswered() => _playerAnswered != -1;

        public abstract string[] GetOptions();
    }
}