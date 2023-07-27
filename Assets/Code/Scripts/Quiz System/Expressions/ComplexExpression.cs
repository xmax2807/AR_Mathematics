using System;
using Project.Utils.MathProvider;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem.Expressions{
    public class ComplexExpression<T> : Expression<T> where T : IComparable<T>
    {
        Expression<T> leftOperand;
        Expression<T> rightOperand;
        char op; // operator
        int unknownIndex; // 0: left, 1: right
        // 1 - (5 - 1) => 1 - -5 + 1
        public ComplexExpression(Expression<T> left, Expression<T> right, char op){
            this.leftOperand = left;
            this.rightOperand = right;
            this.op = op;
        }
        public override T GetUnknownRandomly(Random rand)
        {
            unknownIndex = rand.Next(2); // random 0 or 1
            if(unknownIndex == 0) return leftOperand.GetUnknownRandomly(rand);
            
            // else unknownIndex == 1
            return rightOperand.GetUnknownRandomly(rand);
        }

        public override char GetMostLeftOperator(MathProvider<T> provider) => leftOperand.GetMostLeftOperator(provider);

        public override Expression<T> InvertExpression(MathProvider<T> provider)
        {
            Expression<T> invertedLeft = leftOperand.InvertExpression(provider);
            Expression<T> invertedRight = rightOperand.InvertExpression(provider);

            if(op == '-'){
                char rightOp = invertedRight.GetMostLeftOperator(provider);
                return new ComplexExpression<T>(invertedLeft, invertedRight, provider.MergeOperators(op, rightOp));
            } 

            return new ComplexExpression<T>(invertedLeft, invertedRight, op);
        }
        
        public override string ToString()
        {
            return $"{leftOperand} {op} {rightOperand}";
        }
        public override string GetString()
        {
            return $"{leftOperand.GetString()} {op} {rightOperand.GetString()}";
        }

        public override bool SetValueToUnknown(T value)
        {
            bool result = leftOperand.SetValueToUnknown(value);
            result = result || rightOperand.SetValueToUnknown(value);

            return result;
        }

        public override T Calculate(MathProvider<T> provider)
        {
            T leftValue = this.leftOperand.Calculate(provider);
            T rightValue = this.rightOperand.Calculate(provider);
            return provider.Calculate(leftValue, rightValue, op);
        }
    }
}