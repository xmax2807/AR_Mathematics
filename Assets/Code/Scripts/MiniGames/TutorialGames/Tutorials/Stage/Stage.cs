namespace Project.MiniGames.TutorialGames
{
    public class Stage : IStage
    {
        private Context _context;

        public Stage(Context ctx){
            this._context = ctx;
        }
        public void Begin(){
            
        }
        public void End(){}
        public void Update(){}
    }
}