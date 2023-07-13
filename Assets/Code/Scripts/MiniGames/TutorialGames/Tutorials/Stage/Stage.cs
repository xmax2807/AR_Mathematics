namespace Project.MiniGames.TutorialGames
{
    public class Stage : IStage
    {
        private Context _context;
        private ICommander _commander;
        public Stage(Context ctx, ICommander commander){
            this._context = ctx;
            this._commander = commander;
        }
        public void Begin(){
            UnityEngine.Debug.Log("Begin Stage");
            _commander.GetExecuter().StartCoroutine(_context.CurrentCommand.Execute(_commander));
        }
        public void End(){}
        public void Update(){}
    }
}