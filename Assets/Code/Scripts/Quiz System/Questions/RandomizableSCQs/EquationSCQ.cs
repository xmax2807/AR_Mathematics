using Project.QuizSystem.Expression;
using Project.Utils.MathProvider;
using System;
using System.Linq;

namespace Project.QuizSystem{
    public class EquationSCQ : RandomizableSCQ<int>
    {
        public override QuestionContentType QuestionContentType => QuestionContentType.Text;

        protected override string constQuestion => expression.ToQuest();

        private Expression<int> expression;
        private readonly IntMathProvider mathProvider;
        private readonly int maxNumber;


        public EquationSCQ(int maxNumber, int optionsLength) : base(optionsLength)
        {
            this.maxNumber = maxNumber;
            mathProvider = new IntMathProvider();
            Randomize(random);
        }
        public override void Randomize(Random rand)
        {
            base.Randomize(rand);
            int result = rand.Next(maxNumber + 1);
            expression = Expression<int>.CreateExpression(result, mathProvider);
            _question = expression.ToQuest();

            int range = options.Length;
            for(int i = 0; i < range; i++){
                int value = rand.Next(1,5) + result;
                while(options.Contains(value)){
                    value = rand.Next(-range,range) - 1 + result;
                }
                options[i] = value;
            }
            options[_answer] = result;
        }
        public override string[] GetOptions()
        {
            return options.Select((val)=>val.ToString()).ToArray();       
        }
    }
}