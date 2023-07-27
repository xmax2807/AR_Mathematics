namespace Project.MiniGames.TutorialGames{
    public interface IStageContext{
        void AddCommand();
        void RemoveCommand();
        void ExecuteNextCommand();
    }
}