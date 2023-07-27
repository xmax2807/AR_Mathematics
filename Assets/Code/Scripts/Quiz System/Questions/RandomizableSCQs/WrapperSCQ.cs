using System;
using Project.QuizSystem.SaveLoadQuestion;

namespace Project.QuizSystem{
    public class WrapperSCQ<T> : RandomizableSCQ<T>
    {
        protected IRandomizableQuestion<T> wrappee;
        private QuestionContentType questionContentType;
        public WrapperSCQ(IRandomizableQuestion<T> wrappee, QuestionContentType questionContentType,int optionsLength) : base(optionsLength)
        {
            this.wrappee = wrappee;
            this.questionContentType = questionContentType;
            //options = wrappee.GetRandomOptions(optionsLength);
        }
        protected WrapperSCQ(IRandomizableQuestion<T> wrappee, QuestionContentType questionContentType,T[] options) : this(wrappee,questionContentType,options.Length){}

        public override void Randomize(Random rand = null)
        {
            rand ??= random;
            base.Randomize(rand);
            wrappee.Randomize(rand);
            cacheOptions = null;
            
            options = wrappee.GetRandomOptions(options.Length);
            for(int i = 0; i < options.Length; ++i){
                if(options[_answer].Equals(options[i])){
                    (options[_answer], options[i]) = (options[i], options[_answer]); 
                }
            }
            options[_answer] = wrappee.Answer;
        }

        public override QuestionContentType QuestionContentType => this.questionContentType;

        protected override string constQuestion => "";

        public override string GetQuestion()
        {
            return wrappee.GetQuestion();
        }

        public override string[] GetOptions()
        {
            if(cacheOptions != null) return cacheOptions;
            
            cacheOptions = new string[options.Length];
            for(int i = 0; i < cacheOptions.Length; ++i){
                cacheOptions[i] = wrappee.ConvertOptionToString(options[i]);
            }
            return cacheOptions;
        }

        protected override QuestionSaveData ConvertToData(RandomizableSCQSaveData<T> parent)
        {
            if(wrappee is ISavableQuestion savableWrappee){
                return new WrapperSCQSaveData<T>(savableWrappee.ConvertToData(), parent);
            }
            return parent;
        }

        public override void SetData(QuestionSaveData data)
        {
            if(data is not WrapperSCQSaveData<T> realData){
                base.SetData(data);
                return;
            }

            if(wrappee is ISavableQuestion savableWrappee){
                savableWrappee.SetData(realData.WrappeeData);
            }
            base.SetData(data);
            
            // string[] stringOptions = realData.Options;
            // options = new T[stringOptions.Length];
            // for(int i = 0; i < stringOptions.Length; ++i){
            //     options[i] = wrappee.ParseOptionFromString(stringOptions[i]);
            // }
        }

        protected override RandomizableSCQ<T> DeepClone()
        {
            IRandomizableQuestion<T> cloneWrappee = (IRandomizableQuestion<T>)this.wrappee.Clone();
            return new WrapperSCQ<T>(cloneWrappee,this.questionContentType, this.options);
        }

        protected override T ParseOptionFromString(string data)
        {
            return wrappee.ParseOptionFromString(data);
        }
    }
}