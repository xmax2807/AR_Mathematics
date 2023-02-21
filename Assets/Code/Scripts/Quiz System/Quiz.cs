using System.ComponentModel;
namespace Project.QuizSystem
{
    public class Quiz
    {
        private IQuestion[] questions;
        public event System.Action<IQuestion> OnQuestionChanged;
        private int currentQuest;
        private int CurrentQuest
        {
            get => currentQuest; 
            set
            {
                if (CanGoTo(value))
                {
                    currentQuest = value;
                    OnQuestionChanged?.Invoke(questions[currentQuest]);
                }
            }
        }

        #region Operations
        public void GoNext() => ++CurrentQuest;
        public void GoPrev() => --CurrentQuest;
        public void GoToQuest(int index)=> CurrentQuest = index;

        public bool CanGoNext() => CanGoTo(currentQuest + 1);
        public bool CanGoPrev() => CanGoTo(currentQuest - 1);
        private bool CanGoTo(int index)
        {
            if (questions == null) return false;

            return 0 >= index && index < questions.Length;
        }
        #endregion
    }
}