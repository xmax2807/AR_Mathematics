namespace Project.QuizSystem.SaveLoadQuestion{
    public interface ISavableQuestion{
        QuestionSaveData ConvertToData();
        void SetData(QuestionSaveData data);
    }

    public interface IStringConvertible<T> where T : IStringConvertible<T>{
        string ConvertToString();
    }
}