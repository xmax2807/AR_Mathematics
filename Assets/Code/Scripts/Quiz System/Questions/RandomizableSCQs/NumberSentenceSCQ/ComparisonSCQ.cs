using System;
using System.Linq;
using Project.QuizSystem.Expressions.NumberSentence;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public class ComparisonSCQ : NumberSentenceSCQ
    {
        public ComparisonSCQ(int maxNumber, int optionsLength) : base(maxNumber, optionsLength)
        {
        }

        protected override RandomizableSCQ<int> DeepClone()
        {
            return new ComparisonSCQ(maxNumber,options.Length);
        }

        protected override NumberSentence<int> CreateSentence(Random rand, int sum)
        {
            var leftSide = expressionGenerator.GetExpressionByDepth(rand, sum,1);
            var rightSide = expressionGenerator.GetExpressionByDepth(rand, sum, 0);

            return new Inequality<int>(leftSide, rightSide, FlagExtensionMethods.Randomize<NumberSentenceOperator>(rand));
        }

        protected override void CreateQuestion(Random rand)
        {
            int result = sentence.GetUnknownRandomly(rand);

            _question = sentence.GetFullSentence();
            options[_answer] = result;

            int optionsLength = options.Length;
            int minRange = Math.Max(0, result - optionsLength);
            int maxRange = Math.Min(maxNumber + 1, result + optionsLength);
            
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