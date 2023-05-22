using System;
using Project.Utils.ExtensionMethods;
using Project.Utils.MathProvider;

namespace Project.QuizSystem.Expressions.NumberSentence{
    public enum NumberSentenceOperator{
        Equal = '=', Greater = '>', Less = '<'
    }
    public abstract class NumberSentence<T> where T : IComparable<T>
    {
        public Expression<T> LeftSide {get;protected set;}
        public Expression<T> RightSide {get;protected set;}
        private readonly NumberSentenceOperator seperator;

        public static bool TryParse(string data, MathProvider<T> provider, out NumberSentence<T> result){
            data = data.Replace(" ","");

            NumberSentenceOperator op = NumberSentenceOperator.Equal;
            foreach (NumberSentenceOperator item in Enum.GetValues(typeof(NumberSentenceOperator))){
                int opIndex = data.IndexOf((char)item);
                if (opIndex != -1){
                    op = item;
                    break;
                }
            }

            string[] expressionStrings = data.Split((char)op);
            
            if(expressionStrings.Length != 2){
                result = null;
                return false;
            }

            string leftData = expressionStrings[0];
            string rightData = expressionStrings[1];

            bool parseResult = Expression<T>.TryParse(leftData,provider, out Expression<T> left);
            parseResult = Expression<T>.TryParse(rightData,provider, out Expression<T> right) && parseResult;

            if(parseResult == false){
                result = null;
                return false;
            }

            if(op == NumberSentenceOperator.Equal){
                result = new Equation<T>(left, right);
            }
            else result = new Inequality<T>(left,right, op);

            return true;
        }
        protected NumberSentence(Expression<T> left, Expression<T> right, NumberSentenceOperator sentenceOperator){
            this.LeftSide = left;
            this.RightSide = right;
            this.seperator = sentenceOperator;
        }
        public string GetFullSentence()
        {
            return $"{LeftSide} {(char)seperator} {RightSide}";
        }
        public override string ToString()
        {
            return $"{LeftSide.GetString()} {(char)seperator} {RightSide.GetString()}";
        }

        public int UnknownIndex {get;protected set;}
        public T GetUnknownRandomly(Random rand)
        {
            UnknownIndex = rand.Next(2); // 0 or 1
            if(UnknownIndex == 0) return LeftSide.GetUnknownRandomly(rand);

            // else index == 1
            return RightSide.GetUnknownRandomly(rand);
        }
        public bool SetValueToUnknown(T value){
            bool result = LeftSide.SetValueToUnknown(value);
            result = result || RightSide.SetValueToUnknown(value);
            return result;
        }
    }
}