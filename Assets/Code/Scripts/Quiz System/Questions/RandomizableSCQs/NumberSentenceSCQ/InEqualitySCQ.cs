using System;
using System.Linq;
using Project.QuizSystem.Expressions.NumberSentence;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public class InEqualitySCQ : NumberSentenceSCQ
    {
        private NumberSentenceOperator op;
        int leftSum,rightSum;
        public InEqualitySCQ(int maxNumber, int optionsLength) : base(maxNumber, optionsLength)
        {
            op= NumberSentenceOperator.Equal;
        }

        protected override RandomizableSCQ<int> DeepClone()
        {
            return new InEqualitySCQ(maxNumber,options.Length);
        }

        protected override NumberSentence<int> CreateSentence(Random rand, int sum)
        {
            leftSum = rand.Next(5, maxNumber);
            rightSum = rand.Next(5, maxNumber);

            op = leftSum < rightSum ? NumberSentenceOperator.Less : NumberSentenceOperator.Greater;
        
            var leftSide = expressionGenerator.GetExpressionByDepth(rand, leftSum,0);
            var rightSide = expressionGenerator.GetExpressionByDepth(rand, rightSum, 0);

            return new Inequality<int>(leftSide, rightSide, op);
        }

        protected override void CreateQuestion(Random rand)
        {
            int result = sentence.GetUnknownRandomly(rand);

            _question = sentence.GetFullSentence();
            options[_answer] = result;

            int optionsLength = options.Length;

            int minRange,maxRange;

            if(result == leftSum){
                // ? > 3
                if(op == NumberSentenceOperator.Less){
                    minRange = rightSum;
                    maxRange = rightSum + Math.Max(optionsLength, 5);
                }
                else{
                    minRange = 0;
                    maxRange = rightSum + 1;
                }
            }
            else //if(result == rightSum)
            {
                // 3 < ?
                if(op == NumberSentenceOperator.Less){
                    minRange = 0;
                    maxRange = leftSum + 1;
                }
                // 3 > ?
                else{
                    minRange = leftSum;
                    maxRange = leftSum + Math.Max(optionsLength, 5);
                }
            }

            // 1 + 6 < 5
            // biết nó là 1
            // biết ngưỡng
            // if(op == NumberSentenceOperator.Less){
            //     if(sentence.UnknownIndex == 0){
            //         minRange = result + 1;
            //         maxRange = maxNumber;
            //     }
            //     else{
            //         minRange = 0;
            //         // 4 + ? < 8
            //         // correct : < 4
            //         // wrong : > 4
            //         maxRange = sentence.RightSide.Calculate(mathProvider);
            //     }
            // }
            // else if(op == NumberSentenceOperator.Greater){
            //     if(sentence.UnknownIndex == 0){
            //         minRange = 0;
            //         // 36 + 44 > 80
            //         // correct : > 4
            //         // wrong : <= 8
            //         maxRange = sentence.RightSide.Calculate(mathProvider) - result + 1;
            //     }
            //     else{
            //         minRange = sentence.LeftSide.Calculate(mathProvider) + 1;
            //         maxRange = maxNumber;
            //     }
            // }
            // else{
            //     minRange = Math.Max(0, result - optionsLength);
            //     maxRange = Math.Min(maxNumber + 1, result + optionsLength);
            // }

            for(int i = 0; i < optionsLength; i++){
                if(i == _answer) continue;
                
                int value;
                do{
                    value = rand.Next(minRange, maxRange);
                }
                while(options.Contains(value));

                options[i] = value;
            }
        }
    }
}