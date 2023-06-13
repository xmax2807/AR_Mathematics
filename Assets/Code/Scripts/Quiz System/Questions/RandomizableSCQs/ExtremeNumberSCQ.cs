namespace Project.QuizSystem{
    public class ExtremeNumberSCQ : WrapperSCQ<int>
    {
        public ExtremeNumberSCQ(int optionsLength, int maxNumber, bool isMinimumFinding, int minNumber = 0) 
        : base(new ExtremeNumberQuestion(maxNumber, isMinimumFinding, minNumber), QuestionContentType.Text, optionsLength)
        {
        }

        protected ExtremeNumberSCQ(int optionsLength, IRandomizableQuestion<int> wrappee) :  base(wrappee, QuestionContentType.Text, optionsLength){}
        protected override RandomizableSCQ<int> DeepClone()
        {
            return new ExtremeNumberSCQ(this.options.Length, this.wrappee.Clone() as IRandomizableQuestion<int>);
        }
    }
}