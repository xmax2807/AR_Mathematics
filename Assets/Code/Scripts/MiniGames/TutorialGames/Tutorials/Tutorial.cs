namespace Project.MiniGames.TutorialGames{
    public class Tutorial : ITutorial
    {
        private ContextGroup _contexts;
        private IStage[] _stages;

        private ICommander commander;

        private int _currentStateIndex;

        public Tutorial(ContextGroup group, ICommander commander){
            this.commander = commander;
            
            _contexts = group;
            _currentStateIndex = 0;

            int _stageCount = group.Length;
            _stages = new IStage[_stageCount];
            // for(int i = 0; i < _stageCount; ++i){
            //     _stages[i] = new Stage(_contexts[i], commander);
            // }
        }

        public int CurrentStateIndex => _currentStateIndex;

        public void Begin()
        {
            if(_currentStateIndex >= _stages.Length){
                End();
                UnityEngine.Debug.Log("Tutorial has ended");
                return;
            }
            UnityEngine.Debug.Log("Building stage " + _currentStateIndex);
            _stages[_currentStateIndex] = new Stage(_contexts[_currentStateIndex], commander);
            this.commander.OnStageRestart += RestartStage;
            commander.UpdateCurrentStage(_stages[_currentStateIndex]);
            
            _stages[_currentStateIndex].Begin();
        }

        public void End()
        {
            //Tutorial is finished
            UnityEngine.Debug.Log("Ended Tutorial");
        }

        public void MoveToStage(int index)
        {
            _currentStateIndex = index;
            Begin();
        }

        public void NextStage()
        {
            UnityEngine.Debug.Log("Next Stage");
            this.commander.OnStageRestart -= RestartStage;

            ++_currentStateIndex;
            Begin();
        }

        private void RestartStage(){
            //_stages[_currentStateIndex].Restart();
            --_currentStateIndex;
        }
    }
}