using Project.QuizSystem.Expression;

namespace Project.QuizSystem{
    public class EquationQuest : BaseQuestion
    {
        private Expression<int> expression;
        public EquationQuest(string question, uint answerIndex) : base(question, answerIndex)
        {
            if(!Expression<int>.TryConvertToExpression(question, out expression)){
                throw new System.ArgumentException("Invalid Expression");
            }
        }
    }
}