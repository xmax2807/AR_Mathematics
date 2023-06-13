using System;
using System.Collections.Generic;
using System.Text;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public class ExtremeNumberQuestion : BaseQuestion<int>, IRandomizableQuestion<int>
    {
        private const string HeaderQuestion = "Em hãy tìm số _ trong dãy số:\n";
        private int maxNumber,minNumber;
        private bool isMinimumFinding;
        private int[] numberArray;
        public ExtremeNumberQuestion(string question, int answer) : base(question, answer)
        {
        }

        public ExtremeNumberQuestion(int maxNumber, bool isMinimumFinding = false, int minNumber = 0) : base("", 0){
            if(maxNumber < 10){
                maxNumber = 10;
            }

            this.maxNumber = maxNumber;
            this.minNumber = minNumber;
            this.isMinimumFinding = isMinimumFinding;
        }

        public override QuestionType QuestionType => QuestionType.SingleChoice;

        public override QuestionContentType QuestionContentType => QuestionContentType.Text;

        public override IQuestion Clone()
        {
            return new ExtremeNumberQuestion(maxNumber, isMinimumFinding, minNumber);
        }

        public string ConvertOptionToString(int option)
        {
            return option.ToString();
        }

        public int[] GetRandomOptions(int count)
        {
            System.Random rand = RandomizableSCQ<int>.random ?? new Random();

            int[] result = new int[count];
            for(int i = 0; i < count; ++i){
                int index = rand.Next(0, numberArray.Length);
                result[i] = numberArray[index];
            }
            return result;
        }

        public override string GetQuestion()
        {
            return _question;
        }

        public int ParseOptionFromString(string data)
        {
            if(int.TryParse(data, out int result)){
                return result;
            }
            return -1;
        }

        public IQuestion Random(Random rand = null)
        {
            ExtremeNumberQuestion clone = new(maxNumber, isMinimumFinding, minNumber);
            clone.Randomize(rand);
            return clone;
        }

        public void Randomize(Random rand)
        {
            rand ??= RandomizableSCQ<int>.random;
            
            //Generate numbers
            int count = rand.Next(4,7);
            List<int> listNumbers = new();
            
            int i = 0;
            for(; i < count; ++i){
                int value = rand.Next(minNumber, maxNumber + 1);

                int randomRetries = 6;
                while(randomRetries > 0 && listNumbers.Contains(value)){
                    value = rand.Next(minNumber, maxNumber + 1);
                    --randomRetries;
                }
                listNumbers.Add(value);
            }

            numberArray = listNumbers.ToArray();
            _answer = isMinimumFinding ? numberArray.FindSmallest(Comparer<int>.Default) : numberArray.FindLargest(Comparer<int>.Default);
            
            // Generate question
            StringBuilder builder = new($"<size=50%>{HeaderQuestion}</size>");
            string whichExtreme = isMinimumFinding ? "bé nhất" : "lớn nhất";
            builder.Replace("_", whichExtreme);
            
            i = 0;
            for(; i < count - 1; ++i){
                builder.Append(numberArray[i]).Append(", ");
            }
            builder.Append(numberArray[i]);
            _question = builder.ToString();
        }
    }
}