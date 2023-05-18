using UnityEngine.UI;
using Project.Managers;
using Project.QuizSystem.SaveLoadQuestion;

namespace Project.QuizSystem{
    public interface ISingleChoice 
    {   
        string[] GetOptions();
        int AnswerIndex {get;}
        int UserAnswerIndex {get;}
    }
    public abstract class SingleChoice<T> : BaseQuestion<int>, ISingleChoice, ISavableQuestion
    {
        public override QuestionType QuestionType => QuestionType.SingleChoice;
        protected T[] options;
        protected string[] cacheOptions;
        public int AnswerIndex => _answer;
        public int UserAnswerIndex => _playerAnswered;

        public SingleChoice(string question,T[] options, int answer) : base(question, answer)
        {
            _playerAnswered = -1;
            this.options = options;
        }
        public override bool HasAnswered() => _playerAnswered != -1;

        public abstract string[] GetOptions();

        public abstract QuestionSaveData ConvertToData();
        // {
        //     return new SCQSaveData(){
        //         Options = GetOptions(),
        //         Question = GetQuestion(),
        //         UserAnswerIndex = _playerAnswered,
        //         CorrectAnswerIndex = _answer
        //     };
        // }

        public virtual void SetData(QuestionSaveData data)
        {
            if(data is SCQSaveData scqSaveData){
                _answer = scqSaveData.CorrectAnswerIndex;
                _playerAnswered = scqSaveData.UserAnswerIndex;
            }
            _question = data.Question;
        }

        public T GetAnswer() => options[_answer];
    }
}