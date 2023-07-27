namespace Project.MiniGames.TutorialGames
{
    public abstract class PracticeTask{
        protected string taskQuestion;
        protected string[] _uniqueNames;
        protected int correctIndex;
        public PracticeTask(string[] uniqueNames){
            if(uniqueNames == null){
                throw new System.ArgumentNullException(nameof(uniqueNames));
            }
            _uniqueNames = uniqueNames;
        }
        public void BuildTask(int index = -1){
            correctIndex = index != -1 ? index : UnityEngine.Random.Range(0, _uniqueNames.Length);
            BuildQuestion();
        }
        protected abstract void BuildQuestion();
        public string GetTaskQuestion() => taskQuestion;
        public bool IsCorrect(int index){
            return correctIndex == index;
        }
        public bool IsCorrect(string uniqueName){
            return uniqueName == _uniqueNames[correctIndex];
        }

        public abstract PracticeTask Clone();
    }
}