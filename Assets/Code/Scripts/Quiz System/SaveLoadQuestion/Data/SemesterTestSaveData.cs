namespace Project.QuizSystem.SaveLoadQuestion{
    public class SemesterTestSaveData{
        public string Id {get;set;}
        public string UserId {get;set;}
        public string Title {get;set;}
        public int Semester {get;set;}
        public int NumberOfQuestions {get;set;}
        public int NumberOfCorrections {get;set;}
        public QuestionSaveData[] ListData {get;set;}
    }
}