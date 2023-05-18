using System;
using Project.QuizSystem;
using Project.Utils;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames{
    internal interface IVisitableSCQTask{
        string GetQuestion();
        string[] GetOptions();
        bool IsCorrect(int index);
        int GetAnswerIndex();
    }
    internal interface IVisitableQuizTask<T> {
        T GetAnswer();
    }
    public abstract class RandomizableTask : BaseTask
    {
        protected IQuestion checker;

        public RandomizableTask(int goal, string description) : base(goal, description)
        {
            checker = CreateQuestion().Random();
        }

        protected abstract IRandomizableQuestion CreateQuestion();

        public sealed override bool IsCorrect(object value){
            checker.SetAnswer(value);
            return checker.IsCorrect();
        }
    }
    public class ShapeTask : RandomizableTask{
        //private ShapeQuestion checker;
        public ShapeTask(int goal, string description) : base(goal, description)
        {
            TaskDescription += checker.GetQuestion();
        }

        protected override IRandomizableQuestion CreateQuestion() => new ShapeQuestion("");
    }

    public abstract class RandomizableSingleChoiceTask<T> : RandomizableTask, IVisitableQuizTask<T> where T : System.IEquatable<T>
    {
        protected RandomizableSCQ<T> realQuestion;
        public RandomizableSingleChoiceTask(int goal, string description) : base(goal, description)
        {
        }

         public T GetAnswer()
        {
            return realQuestion.GetAnswer();
        }

        public int GetAnswerIndex() => realQuestion.AnswerIndex;

        public string[] GetOptions()
        {
            return realQuestion.GetOptions();
        }

        public string GetQuestion()
        {
            return realQuestion.GetQuestion();
        }

        public bool IsCorrect(int index)
        {
            return realQuestion.AnswerIndex == index;
        }

        protected sealed override IRandomizableQuestion CreateQuestion(){
            realQuestion = CreateSingleChoiceQuestion();
            realQuestion.Randomize();
            return realQuestion;
        }
        protected abstract RandomizableSCQ<T> CreateSingleChoiceQuestion();
    }

    public class ClockTask : RandomizableSingleChoiceTask<int>{

        public ClockTask(int goal, string description) : base(goal, description)
        {
        }

        protected override RandomizableSCQ<int> CreateSingleChoiceQuestion() => new TimeSCQ(4);
    }
    public class CalendarTask : RandomizableSingleChoiceTask<DateTime>
    {
        public CalendarTask(int goal, string description) : base(goal, description)
        {
        }

        protected override RandomizableSCQ<DateTime> CreateSingleChoiceQuestion() => new DateTimeSCQ();
    }

    public class HouseBuildingTask : RandomizableTask, IVisitableQuizTask<int[]>
    {
        private int[] m_Answer;
        public HouseBuildingTask(int goal, string description) : base(goal, description)
        {
        }

        public int[] GetAnswer()
        {
            return m_Answer;
        }

        protected override IRandomizableQuestion CreateQuestion()
        {
            var instance = new NumberOrderQuestion(TaskDescription,5, 10);
            instance.Randomize();
            m_Answer = instance.Answer;
            return instance;
        }
    }
}