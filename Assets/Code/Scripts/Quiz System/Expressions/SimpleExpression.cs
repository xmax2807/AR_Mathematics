using System;
using Project.Utils.ExtensionMethods;
using Project.Utils.MathProvider;

namespace Project.QuizSystem.Expressions{
    public class SimpleExpression<T> : Expression<T> where T : System.IComparable<T>
    {
        private T number;
        private char op;
        public SimpleExpression(T number, char op){
            this.number = number;
            this.op = op;
            isUnkown = false;
        }
        bool isUnkown;
        public override T GetUnknownRandomly(Random rand)
        {
            isUnkown = true;
            return number;
        }
        public override char GetMostLeftOperator(MathProvider<T> provider)
        {
            return op;
        }

        public override Expression<T> InvertExpression(MathProvider<T> provider)
        {
            char invertedOp = op.InvertOperator();
            return new SimpleExpression<T>(number, invertedOp);
        }

        public override string ToString()
        {
            return isUnkown ? "?" : number.ToString();
        }

        public override string GetString()
        {
            return number.ToString();
        }

        public override bool SetValueToUnknown(T value)
        {
            if(value.CompareTo(this.number) == 0){
                isUnkown = true;
                return true;
            }
            return false;
        }

        public override T Calculate(MathProvider<T> provider)
        {
            return number;
        }
    }
}