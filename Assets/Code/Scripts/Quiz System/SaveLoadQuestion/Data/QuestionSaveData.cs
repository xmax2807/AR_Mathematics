namespace Project.QuizSystem.SaveLoadQuestion{
    public class QuestionSaveData{
        public string Id {get;set;}
        public string Question {get;set;}
    }

    public class SCQSaveData : QuestionSaveData{
        public string[] Options{get;set;}
        public int UserAnswerIndex {get;set;}
        public int CorrectAnswerIndex {get;set;}
    }
    public class UnrandomizableSCQSaveData : SCQSaveData{
        public string ImageURL {get;set;}
    }

    public class RandomizableSCQSaveData<T> : SCQSaveData{
        [Newtonsoft.Json.JsonConstructor]
        public RandomizableSCQSaveData(string[] options, int UserAnswerIndex, int CorrectAnswerIndex){
            Options = options;
            this.UserAnswerIndex = UserAnswerIndex;
            this.CorrectAnswerIndex = CorrectAnswerIndex;
        }
        public RandomizableSCQSaveData(T[] options, int UserAnswerIndex, int CorrectAnswerIndex){
            Options = new string[options.Length];
            for(int i = 0; i < options.Length; i++){
                Options[i] = options[i].ToString();
            }
            this.UserAnswerIndex = UserAnswerIndex;
            this.CorrectAnswerIndex = CorrectAnswerIndex;
        }
        public RandomizableSCQSaveData( RandomizableSCQSaveData<T> clone){
            Options = new string[clone.Options.Length];
            for(int i = 0; i < clone.Options.Length; i++){
                Options[i] = clone.Options[i];
            }
            this.UserAnswerIndex = clone.UserAnswerIndex;
            this.CorrectAnswerIndex = clone.CorrectAnswerIndex;
            this.Question = clone.Question;
        }

        public T[] ConvertToOptionT(System.Func<string, T> callback){
            T[] result = new T[Options.Length];
            for(int i = 0; i < Options.Length; i++){
               result[i] = callback.Invoke(Options[i]);
            }
            return result;
        }
    }

    public class WrapperSCQSaveData<T> : RandomizableSCQSaveData<T>
    {
        public QuestionSaveData WrappeeData{get;set;}

        [Newtonsoft.Json.JsonConstructor]
        public WrapperSCQSaveData(QuestionSaveData data,string[] options, int UserAnswerIndex, int CorrectAnswerIndex) : base(options, UserAnswerIndex, CorrectAnswerIndex)
        {
            this.WrappeeData = data;
        }
        public WrapperSCQSaveData(QuestionSaveData data,RandomizableSCQSaveData<T> parent) : base(parent)
        {
            this.WrappeeData = data;
        }
        
    }

    public class ImageSCQSaveData<T> : RandomizableSCQSaveData<T>{
        [Newtonsoft.Json.JsonConstructor]
        public ImageSCQSaveData(string imageName, string[] options, int UserAnswerIndex, int CorrectAnswerIndex) : base(options, UserAnswerIndex, CorrectAnswerIndex)
        {
            this.ImageName = imageName;
        }
        public ImageSCQSaveData(string imageName, T[] options, int UserAnswerIndex, int CorrectAnswerIndex) : base(options, UserAnswerIndex, CorrectAnswerIndex)
        {
            this.ImageName = imageName;
        }
        public ImageSCQSaveData(string imageName, RandomizableSCQSaveData<T> parent) : base(parent){
            this.ImageName = imageName;
        }

        public string ImageName {get;set;}
    }
    public class TextSCQSaveData<T> : RandomizableSCQSaveData<T>{
        [Newtonsoft.Json.JsonConstructor]
        public TextSCQSaveData(string textContent, string[] options, int UserAnswerIndex, int CorrectAnswerIndex) : base(options, UserAnswerIndex, CorrectAnswerIndex)
        {
            this.TextContent = textContent;
        }
        public TextSCQSaveData(string textContent, T[] options, int UserAnswerIndex, int CorrectAnswerIndex) : base(options, UserAnswerIndex, CorrectAnswerIndex)
        {
            this.TextContent = textContent;
        }
        public TextSCQSaveData(string textContent, RandomizableSCQSaveData<T> parent) : base(parent){
            this.TextContent = textContent;
        }

        public string TextContent{get;set;}
    }
}