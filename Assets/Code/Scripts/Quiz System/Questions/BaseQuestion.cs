
namespace Project.QuizSystem{
    public enum QuestionType{
        SingleChoice, MultipleChoice
    }
    public abstract class BaseQuestion{
        private string _question;
        private uint _answerIndex;
        public BaseQuestion(string question, uint answerIndex){
            _question = question;
            _answerIndex = answerIndex;
        }
        public virtual bool IsCorrect(uint playerAns){
            return playerAns == _answerIndex;
        }
    }
}