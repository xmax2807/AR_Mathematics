using UnityEngine.UI;
using Project.Managers;

namespace Project.QuizSystem{
    public interface ISingleChoice 
    {   
        string[] GetOptions();
    }
    public abstract class SingleChoice<T> : BaseQuestion<int>, ISingleChoice
    {
        public override QuestionType QuestionType => QuestionType.SingleChoice;
        protected T[] options;
        protected string[] cacheOptions;
        public int AnswerIndex => _answer;
        public SingleChoice(string question,T[] options, int answer) : base(question, answer)
        {
            this.options = options;
        }

        public abstract string[] GetOptions();
    }
}