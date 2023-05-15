using Project.Utils.ExtensionMethods;
namespace Project.Utils.MathProvider
{
    public abstract class MathProvider
    {
        public static readonly char[] Operators = new char[] { '+', '-', '*', '/' };
        public static readonly System.Random rand = new(System.DateTime.Now.Millisecond);

        public char MergeOperators(char op1, char op2){
            if(!op1.IsOperator()){
                if(op2.IsOperator()) return op2;
                return ' ';
            }
            if(!op2.IsOperator()){
                if(op1.IsOperator()) return op1;
                return ' ';
            }

            int op1Index = Operators.FindMatch((ch)=>ch == op1);
            int op2Index = Operators.FindMatch((ch)=>ch == op2);

            // if op1 and op2 are neither '+' nor '-'  
            if(op1Index >= 2 && op2Index >= 2){
                return op1Index > op2Index ? op1 : op2;
            }

            if(op1 == op2) return '+';
            return '-';
        }
    }
    public abstract class MathProvider<T> : MathProvider where T : System.IComparable<T>
    {
        public abstract bool IsNegated(T val);
        public abstract T Divide(T a, T b);
        public abstract T Multiply(T a, T b);
        public abstract T Add(T a, T b);
        public abstract T Negate(T a);
        public virtual T Subtract(T a, T b)
        {
            return Add(a, Negate(b));
        }
        public (T, T, char) SplitNumber(T number)
        {
            T val2 = RandomNumberInRange(number);
            int countLimit = 10;
            while(val2.CompareTo(number)==0 && countLimit > 0){
                val2 = RandomNumberInRange(number);
                --countLimit;
            }
            
            T val1;
            char op = Operators[rand.Next(0,2)];
            
            if (op == '-')
            {
                val1 = Add(number, val2);
            }
            else //if (op == '+')
            {
                val1 = Subtract(number, val2);
            }
            return (val1, val2, op);
        }
        protected abstract T RandomNumberInRange(T range);
        public abstract T Parse(string data);
    }

    public class DoubleMathProvider : MathProvider<double>
    {
        public override double Divide(double a, double b)
        {
            return a / b;
        }

        public override double Multiply(double a, double b)
        {
            return a * b;
        }

        public override double Add(double a, double b)
        {
            return a + b;
        }

        public override double Negate(double a)
        {
            return -a;
        }

        protected override double RandomNumberInRange(double range)
        {
            return rand.NextDouble() * range;
        }

        public override bool IsNegated(double val)
        {
            return val < 0.0;
        }

        public override double Parse(string data)
        {
            double.TryParse(data, out double result);
            return result;
        }
    }

    public class IntMathProvider : MathProvider<int>
    {
        public override int Divide(int a, int b)
        {
            return a / b;
        }

        public override int Multiply(int a, int b)
        {
            return a * b;
        }

        public override int Add(int a, int b)
        {
            return a + b;
        }

        public override int Negate(int a)
        {
            return -a;
        }

        protected override int RandomNumberInRange(int range)
        {
            return rand.Next(range);
        }

        public override bool IsNegated(int val)
        {
            return val < 0;
        }
        public override int Parse(string data)
        {
            int.TryParse(data, out int result);
            return result;
        }
    }
}