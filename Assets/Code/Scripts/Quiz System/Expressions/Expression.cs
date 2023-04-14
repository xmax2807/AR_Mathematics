using Project.Utils.MathProvider;
using Project.Utils.ExtensionMethods;
namespace Project.QuizSystem.Expression
{
    public struct Expression<T> where T : System.IComparable<T>
    {
        public T arg1, arg2;
        public char Operator;
        public Expression(T arg1, T arg2, char Operator)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.Operator = Operator;
        }

        public override string ToString()
        {
            return $"{arg1} {Operator} {arg2}";
        }
        public string ToQuest()
        {
            return ToString() + " = ";
        }
        public string FullEquatation() => ToQuest() + "";
        public static Expression<T> ConvertToExpression(string expression)
        {
            int opIndex = FindOperator(expression);
            if(opIndex == -1){
                return new Expression<T>();
            }

            char op = expression[opIndex];

            bool parseRes = expression.Substring(0,opIndex).TryParse<T>(out T arg1);
            parseRes = expression.Substring(opIndex + 1, expression.Length - opIndex - 1).TryParse<T>(out T arg2) && parseRes;

            if(parseRes == false) return new Expression<T>();

            return new Expression<T>(arg1, arg2, op);
        }
        public static bool TryConvertToExpression(string expression, out Expression<T> result)
        {
            result = ConvertToExpression(expression);
            return !result.IsEmpty();
        }
        public static Expression<T> CreateExpression(T result, MathProvider<T> provider){
            (T,T,char) tuple = provider.SplitNumber(result);

            return new Expression<T>(tuple.Item1, tuple.Item2, tuple.Item3);
        }
        public bool IsEmpty() => Operator == '\0';
        private static int FindOperator(string expression){
            for(int i = 0; i < MathProvider.Operators.Length; i++){
                int opIndex = expression.IndexOf(MathProvider.Operators[i]);
                if(opIndex != -1){
                    return opIndex;
                }
            }
            return -1;
        }
    }
}