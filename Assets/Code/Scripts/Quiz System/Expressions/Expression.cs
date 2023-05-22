using Project.Utils.MathProvider;
using Project.Utils.ExtensionMethods;
namespace Project.QuizSystem.Expressions
{
    public abstract class Expression<T> where T : System.IComparable<T>
    {
        public abstract char GetMostLeftOperator(MathProvider<T> provider);
        public abstract T GetUnknownRandomly(System.Random rand);
        public abstract bool SetValueToUnknown(T value);
        public abstract Expression<T> InvertExpression(MathProvider<T> provider);
        public abstract string GetString();

        public abstract T Calculate(MathProvider<T> provider);

        public static bool TryParse(string data, MathProvider<T> provider, out Expression<T> result)
        {
            char op = '+';
            int opIndex = -1;
            foreach (char ch in MathProvider.Operators)
            {
                opIndex = data.IndexOf(ch);
                if (opIndex != -1)
                {
                    op = ch;
                    break;
                }
            }

            if (opIndex == -1)
            {
                T number = provider.Parse(data);
                result = new SimpleExpression<T>(number, provider.IsNegated(number) ? '-' : '+');
                return true;
            }
            string[] subExpressionStrings = data.Split(op, 2);
            
            if(subExpressionStrings.Length < 2) {
                result = null;
                return false;
            }
            
            string leftData = subExpressionStrings[0];
            string rightData = subExpressionStrings[1];


            bool leftOperand = TryParse(leftData,provider, out Expression<T> left);
            bool rightOperand = TryParse(rightData,provider, out Expression<T> right);

            if(leftOperand == false || rightOperand == false){
                result = null;
                return false;
            }

            result = new ComplexExpression<T>(left, right, op);

            return true;
        }
    }
}