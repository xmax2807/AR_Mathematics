using System;
namespace Project.QuizSystem.Expressions.NumberSentence{
    public class Equation<T> : NumberSentence<T> where T : IComparable<T>
    {
        public Equation(Expression<T> left, Expression<T> right) : base(left, right, NumberSentenceOperator.Equal)
        {
        }
    }
}