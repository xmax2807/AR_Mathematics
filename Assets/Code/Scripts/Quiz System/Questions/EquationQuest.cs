using Project.QuizSystem.Expression;

namespace Project.QuizSystem{
    public class EquationQuest : SingleChoice<uint>
    {
        private Expression<int> expression;
        public EquationQuest(string question, uint[] options, uint answerIndex) : base(question,options, answerIndex)
        {
            if(!Expression<int>.TryConvertToExpression(question, out expression)){
                throw new System.ArgumentException("Invalid Expression");
            }
        }
    }
}