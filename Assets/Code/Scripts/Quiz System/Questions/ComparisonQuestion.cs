using System;
using Project.Utils.ExtensionMethods;
using Project.Utils.MathProvider;
using Project.QuizSystem.Expressions;
using Project.QuizSystem.Expressions.NumberSentence;

namespace Project.QuizSystem{
    public class ComparisonQuestion : BaseQuestion<ComparisonQuestion.ExpressionBiggerSide>, IEquatable<ComparisonQuestion>, IRandomizableQuestion
    {
        public enum ExpressionBiggerSide{
            Left,Right
        }
        private int minNumber, maxNumber;
        public int LeftNumber {get;private set;}
        public int RightNumber {get;private set;}
        public ComparisonQuestion(int maxNumber,string question, ExpressionBiggerSide answer, int minNumber = 0) : base(question, answer)
        {
            this.maxNumber = maxNumber;
            this.minNumber = minNumber;
        }
        public ComparisonQuestion(int maxNumber, int minNumber) : this(maxNumber, "", ExpressionBiggerSide.Left, minNumber){}

        protected override void TrySetAnswer(object value)
        {
            if(value is bool booleanValue){
                _answer = booleanValue == true ? ExpressionBiggerSide.Left : ExpressionBiggerSide.Right;
            }
            else if(value is int intValue){
                if(intValue == -1){
                    _answer = ExpressionBiggerSide.Left;
                }
                else if(intValue == 1){
                    _answer = ExpressionBiggerSide.Right;
                }
            }
        }

        public override QuestionType QuestionType => QuestionType.SingleChoice;

        public override QuestionContentType QuestionContentType => QuestionContentType.Text;
        
        public override IQuestion Clone()
        {
            return new ComparisonQuestion(this.maxNumber, this._question, this._answer);
        }

        public bool Equals(ComparisonQuestion other)
        {
            return other._answer.Equals(_answer);
        }

        public IQuestion Random(Random rand = null)
        {
            var clone = new ComparisonQuestion(this.maxNumber, _question, this._answer);
            clone.Randomize(rand);
            return clone;
        }

        public void Randomize(Random rand = null)
        {
            rand ??= new Random();
            _answer = FlagExtensionMethods.Randomize<ExpressionBiggerSide>(rand);

            LeftNumber = rand.Next(minNumber, maxNumber + 1); 
            do{
                RightNumber = rand.Next(minNumber, maxNumber + 1);
            }
            while(RightNumber == LeftNumber);
        }

        public override string GetQuestion() {
            bool isLeftBigger = LeftNumber > RightNumber;
            if(_answer == ExpressionBiggerSide.Left){
                _question = isLeftBigger == true ? "Bé hãy tìm số lớn hơn" : "Bé hãy tìm số bé hơn";
            }
            else{ // if _answer == ExpressionBiggerSide.Right
                _question = isLeftBigger == false ? "Bé hãy tìm số lớn hơn" : "Bé hãy tìm số bé hơn";
            }

            return _question;
        }
    }
}