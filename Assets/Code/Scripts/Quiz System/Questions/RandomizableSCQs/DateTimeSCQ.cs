using System;
using System.Linq;
using Project.QuizSystem.SaveLoadQuestion;
using Project.Utils.ExtensionMethods;

namespace Project.QuizSystem{
    public class DateTimeSCQ : RandomizableSCQ<System.DateTime>
    {
        public static string[] vietnameseDayOfWeeks = {
            "Chủ Nhật",
            "Thứ Hai",
            "Thứ Ba",
            "Thứ Tư",
            "Thứ Năm",
            "Thứ Sáu",
            "Thứ Bảy",
        };
        public DateTimeSCQ(int optionsLength = 4) : base(optionsLength)
        {
            if(optionsLength > 4){
                options = new System.DateTime[4];
            }
        }
        public DateTime GetAnswer() => options[_answer];
        public override string[] GetOptions()
        {
            string[] result = new string[options.Length];
            for(int i = 0; i < result.Length; i++){
                DateTime opt = options[i];
                result[i] = $"{vietnameseDayOfWeeks[(int)opt.DayOfWeek]} ngày {opt.Day}";
            }

            return result;    
        }
        public override void Randomize(Random rand = null)
        {
             rand ??= random;
            _answer = rand.Next(options.Length);
            
            int range = options.Length %2 == 0? options.Length / 2 : options.Length / 2 + 1;
            DateTime randomDate = DateTimeExtensionMethods.RandomDayWithinAYear();
            for(int i = 0; i < range; i++){
                options[i] = randomDate.AddDays(-range + i);
                options[range + i] = randomDate.AddDays(range + i);
            }
        }

        protected override RandomizableSCQ<DateTime> DeepClone()
        {
            return new DateTimeSCQ(this.options.Length);
        }

        protected override DateTime ParseFromString(string data)
        {
            if(DateTime.TryParse(data, out DateTime result)){
                return result;
            }
            return DateTime.MinValue;
        }

        protected override QuestionSaveData ConvertToData(RandomizableSCQSaveData<DateTime> parent)
        {
            return new ImageSCQSaveData<DateTime>("", parent);
        }

        public override QuestionContentType QuestionContentType => QuestionContentType.Text;

        protected override string constQuestion => "Em hãy chỉ đúng thứ ngày tháng trên tờ lịch";
    }
}