using System;
using Project.QuizSystem.SaveLoadQuestion;
using Project.Utils;

namespace Project.QuizSystem{
    public interface IRandomizableQuestion : IRandomizable{
        IQuestion Random(Random rand = null);
        IQuestion GetClone();
    }
    
    public abstract class RandomizableSCQ<T> : SingleChoice<T>, IRandomizableQuestion where T : IEquatable<T>
    {
        protected static readonly System.Random random = new(DateTime.Now.Millisecond);
        protected abstract string constQuestion {get;}
        public RandomizableSCQ(int optionsLength) : base("", new T[optionsLength],0){}

        public bool Equals(RandomizableSCQ<T> other)
        {
            return this.options[_answer].Equals(other.options[_answer]);
        }

        public override string[] GetOptions()
        {
            if(cacheOptions != null) return cacheOptions;

            cacheOptions = new string[options.Length];
            for(int i = 0; i < options.Length; i++){
                cacheOptions[i] = options[i].ToString();
            }

            return cacheOptions;
        }

        public virtual void Randomize(Random rand = null)
        {
            _answer = random.Next(0, options.Length);
        }
        public override string GetQuestion()
        {
            return constQuestion;
        }

        public override void SetData(QuestionSaveData data)
        {
            if(data is RandomizableSCQSaveData<T> realData){
                options = realData.ConvertToOptionT(ParseFromString);
                
            }
            base.SetData(data);
        }
        protected abstract T ParseFromString(string data);
        public sealed override QuestionSaveData ConvertToData()
        {
            var parent = new RandomizableSCQSaveData<T>(options, _answer, _playerAnswered)
            {
                Question = GetQuestion()
            };
            return ConvertToData(parent);
        }
        protected abstract QuestionSaveData ConvertToData(RandomizableSCQSaveData<T> parent);

        public IQuestion Random(Random rand) => RandomT(rand);
        public sealed override IQuestion Clone()
        {
            var instance = DeepClone();
            return instance;
        }
        protected abstract RandomizableSCQ<T> DeepClone();
        public IQuestion GetClone() => Clone();

        public RandomizableSCQ<T> RandomT(Random rand = null)
        {
            var instance = DeepClone();
            instance.Randomize(rand);
            return instance;
        }
    }
}