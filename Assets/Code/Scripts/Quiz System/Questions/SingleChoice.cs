namespace Project.QuizSystem{
    public class SingleChoice<T> : BaseQuestion<uint>
    {
        protected T[] options;
        public SingleChoice(string question,T[] options, uint answer) : base(question, answer)
        {
            this.options = options;
        }
    }
}