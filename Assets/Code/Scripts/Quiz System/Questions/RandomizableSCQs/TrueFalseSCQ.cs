using System;
using Project.QuizSystem.SaveLoadQuestion;

namespace Project.QuizSystem{
    public class TrueFalseSCQ<T> : RandomizableSCQ<string>{
        BaseQuestion<T> trueQuestion;
        BaseQuestion<T> falseQuestion;
        private string questionTitle;
        public TrueFalseSCQ(string question) : base(2){
            //answer will be either 0 or 1
            questionTitle = question;
            options = new string[2]{"Đúng","Sai"};
            _answer = 0;
        }

        public override QuestionContentType QuestionContentType => trueQuestion.QuestionContentType;

        protected override string constQuestion => questionTitle;

        public override void SetData(QuestionSaveData data)
        {
            base.SetData(data);
        }

        public override string[] GetOptions()
        {
            return options;
        }

        public override string GetQuestion() => _answer == 0? trueQuestion.GetQuestion() : falseQuestion.GetQuestion();

        protected override string ParseOptionFromString(string data)
        {
            throw new System.NotImplementedException();
        }

        protected override QuestionSaveData ConvertToData(RandomizableSCQSaveData<string> parent)
        {
            throw new System.NotImplementedException();
        }

        protected override RandomizableSCQ<string> DeepClone()
        {
            throw new System.NotImplementedException();
        }
    }
}