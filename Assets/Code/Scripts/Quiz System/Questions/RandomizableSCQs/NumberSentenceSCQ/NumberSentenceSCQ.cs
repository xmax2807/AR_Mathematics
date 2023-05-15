using System;
using System.Linq;
using Project.QuizSystem.Expressions;
using Project.QuizSystem.Expressions.NumberSentence;
using Project.QuizSystem.SaveLoadQuestion;
using Project.Utils.MathProvider;

namespace Project.QuizSystem{
    public abstract class NumberSentenceSCQ : RandomizableSCQ<int>
    {
        public override QuestionContentType QuestionContentType => QuestionContentType.Text;

        protected override string constQuestion => _question;

        protected NumberSentence<int> sentence;
        protected static readonly IntMathProvider mathProvider = new();
        protected readonly IntExpressionGenerator expressionGenerator;
        protected readonly int maxNumber;
        protected NumberSentenceSCQ(int maxNumber, int optionsLength) : base(optionsLength)
        {
            this.maxNumber = Math.Max(maxNumber, 10);
            expressionGenerator = new IntExpressionGenerator(maxNumber, mathProvider);
        }
        protected override abstract RandomizableSCQ<int> DeepClone();
        public sealed override void Randomize(Random rand = null)
        {
            rand ??= random;
            base.Randomize(rand);
            int sum = rand.Next(5,maxNumber + 1);
            this.sentence = CreateSentence(rand, sum);
            CreateQuestion(rand);
        }

        protected abstract NumberSentence<int> CreateSentence(Random rand, int sum);
        protected abstract void CreateQuestion(Random rand);

        public override string GetQuestion()
        {
            return sentence?.GetFullSentence();
        }

        public override string[] GetOptions()
        {
            return options.Select((val)=>val.ToString()).ToArray();   
        }
        protected override int ParseFromString(string data)
        {
            if(int.TryParse(data, out int result)){
                return result;
            }
            return -1;
        }
        public override void SetData(QuestionSaveData data)
        {
            base.SetData(data);
            if(data is not TextSCQSaveData<int> textData){
                return;
            }
            NumberSentence<int>.TryParse(textData.TextContent,mathProvider, out sentence);
            sentence?.SetValueToUnknown(options[_answer]);

            UnityEngine.Debug.Log(sentence.GetFullSentence());
        }
        protected override QuestionSaveData ConvertToData(RandomizableSCQSaveData<int> parent)
        {
            return new TextSCQSaveData<int>(sentence.ToString(), parent);
        }
    }
}