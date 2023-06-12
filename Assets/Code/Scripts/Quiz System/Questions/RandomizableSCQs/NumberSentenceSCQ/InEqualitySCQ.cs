using System;
using System.Linq;
using Project.QuizSystem.Expressions.NumberSentence;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public class InEqualitySCQ : NumberSentenceSCQ
    {
        private NumberSentenceOperator op;
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
            this.op = FlagExtensionMethods.Randomize<NumberSentenceOperator>(rand);
            int lessSum = Math.Max(sum - rand.Next(sum / 2), 2);
            int greaterSum = Math.Min(sum + rand.Next(sum / 2), maxNumber);

            int leftSum = sum, rightSum = sum;
            if(op == NumberSentenceOperator.Less){
                leftSum = lessSum;
                rightSum = greaterSum;
            }
            else if(op == NumberSentenceOperator.Greater){
                leftSum = greaterSum;
                rightSum = lessSum;
            }
        
            var leftSide = expressionGenerator.GetExpressionByDepth(rand, leftSum,1);
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
            minRange = Math.Max(0, result - optionsLength);
            maxRange = Math.Min(maxNumber + 1, result + optionsLength);

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