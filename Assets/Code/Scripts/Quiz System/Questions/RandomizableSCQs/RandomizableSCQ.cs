using System;
using Project.Utils;

namespace Project.QuizSystem{
    public abstract class RandomizableSCQ<T> : SingleChoice<T>, IRandomizable<RandomizableSCQ<T>> where T : IEquatable<T>
    {
        protected static readonly System.Random random = new();
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
    }
}