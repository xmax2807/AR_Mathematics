namespace Project.QuizSystem{
    public class NumberOrderSCQ : WrapperSCQ<int[]>
    {
        public NumberOrderSCQ(int optionsLength, int maxNumber) : base(new NumberOrderQuestion("", maxNumber), QuestionContentType.Text, optionsLength)
        {
        }
        public override QuestionContentType QuestionContentType => QuestionContentType.Text;
    }
}