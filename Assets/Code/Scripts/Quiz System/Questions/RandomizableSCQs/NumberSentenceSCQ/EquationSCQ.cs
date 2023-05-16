using Project.QuizSystem.Expressions.NumberSentence;
using Project.QuizSystem.Expressions;
using Project.Utils.MathProvider;
using System;
using System.Linq;

namespace Project.QuizSystem{
    public class EquationSCQ : NumberSentenceSCQ
    {
        public EquationSCQ(int maxNumber, int optionsLength) : base(maxNumber,optionsLength)
        {
            if(optionsLength >= maxNumber){
                options = new int[4];
            }
            // this.maxNumber = Math.Max(maxNumber, 10);
            // mathProvider = new IntMathProvider();
            // expressionGenerator = new IntExpressionGenerator(maxNumber, mathProvider);
            // Randomize(random);
        }

        protected override RandomizableSCQ<int> DeepClone()
        {
            return new EquationSCQ(this.maxNumber, this.options.Length);
        }

        protected override NumberSentence<int> CreateSentence(Random rand, int sum)
        {
            var leftSide = expressionGenerator.GetExpressionByDepth(rand, sum,1);
            var rightSide = expressionGenerator.GetExpressionByDepth(rand, sum, 0);
            return new Equation<int>(leftSide, rightSide);
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