using Project.QuizSystem;
using Project.Utils;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames{
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
}