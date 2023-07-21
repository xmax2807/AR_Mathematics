namespace Project.MiniGames.TutorialGames{
    public class WhichIsPracticeTask : PracticeTask
    {
        private string _format;
        public WhichIsPracticeTask(string format, string[] uniqueNames) : base(uniqueNames){
            _format = format;
        }

        public override PracticeTask Clone()
        {
            return new WhichIsPracticeTask(_format, _uniqueNames);
        }

        protected override void BuildQuestion()
        {
            taskQuestion = _format.Replace("{0}", _uniqueNames[correctIndex]);
        }
    }
}