using System;
using System.Linq;
using Project.QuizSystem.SaveLoadQuestion;

namespace Project.QuizSystem{
    public class TimeSCQ : RandomizableSCQ<int>
    {
        public TimeSCQ(int optionsLength) : base(optionsLength)
        {
            if(optionsLength > 12){
                options = new int [12];
            }
        }

        public override QuestionContentType QuestionContentType => QuestionContentType.Text;

        protected override string constQuestion => "Đồng hồ chỉ mấy giờ ?";

        public override void Randomize(Random rand = null)
        {
            rand ??= random;
            _answer = rand.Next(options.Length);
            options = Enumerable.Range(1,12).OrderBy(x => rand.Next()).Take(options.Length).ToArray();
        }
        public int GetAnswer() => options[_answer];

        public override string[] GetOptions()
        {
            string[] options = base.GetOptions();
            string[] result = new string[options.Length];
            for(int i = 0 ;i < options.Length; i++){
                result[i] = options[i] + " giờ";
            }
            return result;
        }

        protected override RandomizableSCQ<int> DeepClone()
        {
            return new TimeSCQ(this.options.Length);
        }

        protected override int ParseFromString(string data)
        {
            bool parseResult = int.TryParse(data, out int result);
            if(parseResult == false) return -1;
            return result;
        }

        protected override QuestionSaveData ConvertToData(RandomizableSCQSaveData<int> parent)
        {
            return new ImageSCQSaveData<int>("", parent);
        }
    }
}