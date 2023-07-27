namespace Project.MiniGames.TutorialGames{
    public interface IStage{
        void Begin();
        void Update();
        void End();
        void MoveToCommand(int index);
        int CurrentCommandIndex {get;}
    }
}