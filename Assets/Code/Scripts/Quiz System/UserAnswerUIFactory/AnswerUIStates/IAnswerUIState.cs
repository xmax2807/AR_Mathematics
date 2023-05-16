namespace Project.QuizSystem.UIFactory{
    public enum AnswerUIState{
        NotAnswered, Answered, Result
    }
    public interface IAnswerUIState{
        void ChangeUI(AnswerUIState state);
        
    }
}