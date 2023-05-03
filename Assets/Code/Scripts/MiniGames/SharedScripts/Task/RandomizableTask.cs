using Project.QuizSystem;
using Project.Utils;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames{
    internal interface IVisitableQuizTask{
        string GetQuestion();
        string[] GetOptions();
        bool IsCorrect(int index);
    }
    internal interface IVisitableQuizTask<T> : IVisitableQuizTask{
        T GetAnswer();
    }
    public class ShapeTask : BaseTask{
        private ShapeQuestion checker;
        public ShapeTask(int goal, string description) : base(goal, description)
        {
            checker = new ShapeQuestion("");
            TaskDescription += checker.GetQuestion();
        }

        public override bool IsCorrect(object value)
        {
            if(value is not Shape.ShapeType type) return false;
            
            return checker.IsCorrect(new Shape(type));
        }
    }

    public class ClockTask : BaseTask, IVisitableQuizTask<int>{
        private TimeSCQ checker;

        public ClockTask(int goal, string description) : base(goal, description)
        {
            checker = new TimeSCQ(4);
        }

        public int GetAnswer()
        {
            return checker.GetAnswer();
        }

        public string[] GetOptions()
        {
            return checker.GetOptions();
        }

        public string GetQuestion()
        {
            return checker.GetQuestion();
        }

        public override bool IsCorrect(object value)
        {
            if(value is not int hour) return false;
            return checker.IsCorrect(hour);
        }

        public bool IsCorrect(int index)
        {
            return checker.AnswerIndex == index;
        }
    }
}