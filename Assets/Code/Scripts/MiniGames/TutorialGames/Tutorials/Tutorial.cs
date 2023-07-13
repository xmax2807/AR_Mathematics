namespace Project.MiniGames.TutorialGames{
    public class Tutorial : ITutorial
    {
        private ContextGroup _contexts;
        private IStage[] _stages;

        private int _currentStateIndex;

        public Tutorial(ContextGroup group){
            _contexts = group;
            _currentStateIndex = 0;
            _stages = new IStage[group.Length];
            for(int i = 0; i < _stages.Length; ++i){
                _stages[i] = new Stage(_contexts[i]);
            }
        }
        public void Begin()
        {
            if(_currentStateIndex >= _stages.Length){
                End();
                return;
            }
            _stages[_currentStateIndex].Begin();
        }

        public void End()
        {
            //Tutorial is finished
            UnityEngine.Debug.Log("Ended Tutorial");
        }

        public void NextStage()
        {
            ++_currentStateIndex;
            Begin();
        }
    }
}