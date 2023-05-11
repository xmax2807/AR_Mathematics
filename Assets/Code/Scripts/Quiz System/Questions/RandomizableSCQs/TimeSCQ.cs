using System;
using System.Linq;

namespace Project.QuizSystem{
    public class TimeSCQ : RandomizableSCQ<int>
    {
        public TimeSCQ(int optionsLength) : base(optionsLength)
        {
            if(optionsLength > 12){
                options = new int [12];
            }
            Randomize(random);
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

        public override IQuestion Clone()
        {
            return new TimeSCQ(this.options.Length);
        }
    }
}