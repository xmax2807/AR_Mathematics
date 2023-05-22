using System;

namespace Project.QuizSystem{
    public class ExtremeNumberQuestion : BaseQuestion<int[]>, IRandomizableQuestion
    {
        
        public ExtremeNumberQuestion(string question, int[] answer) : base(question, answer)
        {
        }

        public override QuestionType QuestionType => QuestionType.SingleChoice;

        public override QuestionContentType QuestionContentType => throw new NotImplementedException();

        public override IQuestion Clone()
        {
            throw new NotImplementedException();
        }

        public IQuestion GetClone()
        {
            throw new NotImplementedException();
        }

        public IQuestion Random(Random rand = null)
        {
            throw new NotImplementedException();
        }

        public void Randomize(Random rand)
        {
            throw new NotImplementedException();
        }
    }
}