using System;
namespace Project.QuizSystem.Expressions.NumberSentence{
    public class Inequality<T> : NumberSentence<T> where T : IComparable<T>
    {
        public Inequality(Expression<T> left, Expression<T> right, NumberSentenceOperator sentenceOperator) : base(left, right, sentenceOperator)
        {
        }
        public static Inequality<T> LessSentence(Expression<T> left, Expression<T> right) => new(left, right, NumberSentenceOperator.Less);
        public static Inequality<T> GreatherSentence(Expression<T> left, Expression<T> right) => new(left, right, NumberSentenceOperator.Greater);
        public static Inequality<T> EqualSentence(Expression<T> left, Expression<T> right) => new(left, right, NumberSentenceOperator.Equal);

    }
}