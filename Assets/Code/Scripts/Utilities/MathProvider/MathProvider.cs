namespace Project.Utils.MathProvider
{
    public abstract class MathProvider
    {
        public static readonly char[] Operators = new char[] { '+', '-', '*', '/' };
        public static readonly System.Random rand = new();
    }
    public abstract class MathProvider<T> : MathProvider where T : System.IComparable<T>
    {
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
            T val1 = RandomNumberInRange(number);
            T val2;
            char op;
            if (val1.CompareTo(number) > 1)
            {
                val2 = Subtract(val1, number);
                op = '-';
            }
            else
            {
                val2 = Subtract(number, val1);
                op = '+';
            }
            return (val1, val2, op);
        }
        protected abstract T RandomNumberInRange(T range);
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
            return rand.NextDouble() * range * 1.5;
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
            return rand.Next((int) 1.5f * range);
        }
    }
}