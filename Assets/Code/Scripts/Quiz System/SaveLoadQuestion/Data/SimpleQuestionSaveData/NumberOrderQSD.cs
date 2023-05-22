namespace Project.QuizSystem.SaveLoadQuestion{
    public class NumberOrderQSD : QuestionSaveData{
        public int Count{get;set;}
        public int MaxNumber {get;set;}
        public bool IsAscending {get;set;}
        public int[] Numbers {get;set;}
    }
}