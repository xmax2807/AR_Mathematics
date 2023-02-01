namespace Project.Utils.MathProvider{
    public abstract class MathProvider{
        public static readonly char[] Operators = new char[] {'+', '-', '*', '/'};
    }
    public abstract class MathProvider<T> : MathProvider
    {
        public abstract T Divide(T a, T b);
        public abstract T Multiply(T a, T b);
        public abstract T Add(T a, T b);
        public abstract T Negate(T a);
        public virtual T Subtract(T a, T b)
        {
            return Add(a, Negate(b));
        }
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
    }
}