using Project.UI.QuizAnswerUI;

namespace Project.QuizSystem.UIFactory{
    public abstract class BaseAnswerUIState : IAnswerUIState
    {
        public void ChangeUI(AnswerUIState state)
        {
            switch(state){
                case AnswerUIState.NotAnswered: NotAnsweredUI();
                break;
                case AnswerUIState.Answered: AnsweredUI();
                break;
                case AnswerUIState.Result: ResultUI();
                break;
            }
        }

        protected abstract void NotAnsweredUI();
        protected abstract void AnsweredUI();
        protected abstract void ResultUI();
        public abstract void ChangeUserAnswer(object answer);
    }

    public class ButtonAnswerUIState : BaseAnswerUIState
    {
        private ButtonAnswerUI[] group;
        private int correctAnswer;
        private int userAnsweredIndex;
        public ButtonAnswerUIState(int correctAnswerIndex, ButtonAnswerUI[] options){
            correctAnswer = correctAnswerIndex;
            group = options;
        }

        public override void ChangeUserAnswer(object answer)
        {
            if(answer is not int answerIndex){
                return;
            }
            ChangeUserAnsweredIndex(answerIndex);
        }

        private void ChangeUserAnsweredIndex(int index) => userAnsweredIndex = index;
        protected override void AnsweredUI()
        {
            foreach(ButtonAnswerUI button in group){
                button.Reset();
            }
            group[userAnsweredIndex].ChangeToSelectState();
        }

        protected override void NotAnsweredUI()
        {
            foreach(ButtonAnswerUI button in group){
                button.Reset();
            }
        }

        protected override void ResultUI()
        {
            foreach(ButtonAnswerUI button in group){
                button.Reset();
            }
            bool isCorrect = correctAnswer == userAnsweredIndex;
            if(!isCorrect){
                group[correctAnswer].ChangeToTrueFalseState(true);
            }
            group[userAnsweredIndex].ChangeToTrueFalseState(isCorrect);
        }
    }
}