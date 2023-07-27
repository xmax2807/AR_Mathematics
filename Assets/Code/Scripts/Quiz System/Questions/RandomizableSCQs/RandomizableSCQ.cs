using System;
using Project.QuizSystem.SaveLoadQuestion;
using Project.Utils;

namespace Project.QuizSystem{
    public interface IRandomizableQuestion : IQuestion, IRandomizable{
        IQuestion Random(Random rand = null);
    }
    public interface IRandomizableQuestion<T> : IRandomizableQuestion, IRandomizableOptions<T>{}
    
    public abstract class RandomizableSCQ<T> : SingleChoice<T>, IRandomizableQuestion
    {
        public static readonly System.Random random = new(DateTime.Now.Millisecond);
        protected abstract string constQuestion {get;}
        public RandomizableSCQ(int optionsLength) : base("", new T[optionsLength],0){}

        // public bool Equals(RandomizableSCQ<T> other)
        // {
        //     return this.options[_answer].Equals(other.options[_answer]);
        // }

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
                options = realData.ConvertToOptionT(ParseOptionFromString);
                //UnityEngine.Debug.Log($"{options.Length}");
            }
            base.SetData(data);
        }
        protected abstract T ParseOptionFromString(string data);
        public sealed override QuestionSaveData ConvertToData()
        {
            string[] stringOptions = ConvertOptionsToStrings();
            // foreach(string str in stringOptions){
            //     UnityEngine.Debug.Log(str);
            // }
            var parent = new RandomizableSCQSaveData<T>(stringOptions, UserAnswerIndex: _playerAnswered, CorrectAnswerIndex: _answer)
            {
                Question = GetQuestion()
            };
            return ConvertToData(parent);
        }
        protected abstract QuestionSaveData ConvertToData(RandomizableSCQSaveData<T> parent);

        // for the case like an array, this method can be customized to convert an array to string
        // This should be override for any class use T as an array
        protected virtual string[] ConvertOptionsToStrings(){
            string[] result = new string[options.Length];
            for(int i = 0; i < options.Length; ++i){
                result[i] = options[i].ToString();
            }
            return result;
        }

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