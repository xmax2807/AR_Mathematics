using Project.Utils.MathProvider;
using System;

namespace Project.QuizSystem.Expressions
{
    public abstract class ExpressionGenerator<T> where T : IComparable<T>
    {
        protected T limit;
        protected MathProvider<T> mathProvider;
        protected int expressionDepth;
        public ExpressionGenerator(T limitValue, MathProvider<T> mathProvider)
        {
            this.limit = limitValue;
            this.mathProvider = mathProvider;
            expressionDepth = 2;
        }
        public ExpressionGenerator(int expressionDepth,T limitValue, MathProvider<T> mathProvider) : this(limitValue, mathProvider){
            this.expressionDepth = expressionDepth;   
        }
        public Expression<T> PickAnExpressionVariant(Random rand, T result)
        {
            int randomIndex = rand.Next(2);
            switch (randomIndex)
            {
                //case 0: return StandardExpression(rand);
                case 0: return CreateComplexExpression(result, rand.Next(0, expressionDepth + 1), rand);
                case 1: return CreateComplexExpression(result, rand.Next(0, expressionDepth + 1), rand);
                default:
                    break;
            }
            return CreateSimpleExpression(result);
        }
        public Expression<T> GetExpressionByDepth(Random rand, T result, int depth){
            return CreateComplexExpression(result, depth, rand);
        }
        protected abstract Expression<T> CreateSimpleExpression(T result);
        protected abstract Expression<T> CreateComplexExpression(T result, int depth, Random rand);
    }

    public class IntExpressionGenerator : ExpressionGenerator<int>
    {
        public IntExpressionGenerator(int limitValue, MathProvider<int> mathProvider) : base(limitValue, mathProvider)
        {
            if(limitValue <= 3){
                limit = 10;
            }
        }

        protected override Expression<int> CreateComplexExpression(int result, int depth, Random rand)
        {
            if(result <= 2 || depth <= 0) return CreateSimpleExpression(result);

            int firstNumber = rand.Next(3,limit);
            int secondNumber;
            char op;
            
            if (firstNumber > result){
                secondNumber = firstNumber - result;
                op = '-';
            }
            else{
                secondNumber = result - firstNumber;
                op = '+';
            }

            Expression<int> leftOperand = CreateComplexExpression(firstNumber, --depth, rand);
            Expression<int> rightOperand = CreateComplexExpression(secondNumber, --depth, rand);

            if(op == '-'){
                rightOperand = rightOperand.InvertExpression(mathProvider);
                op = mathProvider.MergeOperators('+', rightOperand.GetMostLeftOperator(mathProvider));
            }

            return new ComplexExpression<int>(leftOperand, rightOperand, op);
        }

        protected override Expression<int> CreateSimpleExpression(int result)
        {
            return new SimpleExpression<int>(result, result < 0 ? '-' : '+');
        }
    }
}