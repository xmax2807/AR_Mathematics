using System.Text;

namespace Project.QuizSystem{
    public class NumberOrderSCQ : WrapperSCQ<int[]>
    {
        public NumberOrderSCQ(int optionsLength, int maxNumber, bool IsDescending) : base(new NumberOrderQuestion("", maxNumber, IsDescending), QuestionContentType.Text, optionsLength)
        {
        }
        protected NumberOrderSCQ(int optionsLength, IRandomizableQuestion<int[]> wrappee) :  base(wrappee, QuestionContentType.Text, optionsLength){}
        public override QuestionContentType QuestionContentType => QuestionContentType.Text;

        protected override RandomizableSCQ<int[]> DeepClone()
        {
            return new NumberOrderSCQ(this.options.Length, this.wrappee);
        }

        protected override string[] ConvertOptionsToStrings()
        {
            //UnityEngine.Debug.Log("This overrided is called");
            string[] result = new string[options.Length];

            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < result.Length; ++i){
                builder.Clear();
                int j = 0;
                for(; j < options[i].Length - 1; ++j){
                    builder.Append(options[i][j]).Append(',');
                }

                builder.Append(options[i][j]);
                result[i] = builder.ToString();
            }

            // UnityEngine.Debug.Log("Converting to datas");
            // foreach(string res in result){
            //     UnityEngine.Debug.Log(res);
            // }

            return result;
        }
    }
}