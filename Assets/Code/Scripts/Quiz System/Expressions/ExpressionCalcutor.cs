using Project.Utils.MathProvider;
namespace Project.QuizSystem.Expression{
    public static class ExpressionCalculator{
        public static T Calculate<T>(Expression<T> expression, MathProvider<T> provider) where T: System.IComparable<T>{
            char op = expression.Operator;
            return op switch
            {
                '+' => provider.Add(expression.arg1, expression.arg2),
                '-' => provider.Subtract(expression.arg1, expression.arg2),
                '*' => provider.Multiply(expression.arg1, expression.arg2),
                '/' => provider.Divide(expression.arg1, expression.arg2),
                _ => default,
            };
        }
    }
}