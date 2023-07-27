using System;
using System.Collections.Generic;
using System.Text;
using Project.Utils.ExtensionMethods;

namespace Project.MiniGames.TutorialGames{
    public class WhichIsPracticeTask : PracticeTask
    {
        private string _format;
        public WhichIsPracticeTask(string format, string[] uniqueNames) : base(uniqueNames){
            _format = format;
        }

        public override PracticeTask Clone()
        {
            return new WhichIsPracticeTask(_format, _uniqueNames);
        }

        protected override void BuildQuestion()
        {
            taskQuestion = _format.Replace("{0}", _uniqueNames[correctIndex]);
        }
    }

    public class WhereIsPracticeTask : PracticeTask
    {
        public enum WhichDirectionType{
            Left, Right, Front, Back, Up, Down
        }
        
        //create Dictionary from Direction type and map to string
        private static Dictionary<WhichDirectionType, string> _directionNames = new()
        {
            {WhichDirectionType.Left, "bên trái"},
            {WhichDirectionType.Right, "bên phải"},
            {WhichDirectionType.Front, "đằng trước"},
            {WhichDirectionType.Back, "đằng sau"},
            {WhichDirectionType.Up, "bên trên"},
            {WhichDirectionType.Down, "bên dưới"}
        };

        public enum LimitDirection
        {
            Horizontal,Vertical
        }

        private string _format;
        private string _vocative;
        private LimitDirection _limitDirection;
        public WhereIsPracticeTask(string format, string vocative, LimitDirection limitDirection, string[] uniqueNames) : base(uniqueNames)
        {
            _format = format;
            _limitDirection = limitDirection;
            _vocative = vocative;
        }

        public override PracticeTask Clone()
        {
            return new WhereIsPracticeTask(_format, _vocative, _limitDirection, _uniqueNames);
        }

        protected override void BuildQuestion()
        {
            StringBuilder builder = new(_format);

            builder.Replace("{0}", _uniqueNames[correctIndex]);

            int relativeIndex = UnityEngine.Random.Range(0, _uniqueNames.Length);
            while(relativeIndex == correctIndex){
                relativeIndex = UnityEngine.Random.Range(0, _uniqueNames.Length);
            }

            WhichDirectionType type;
            if (_limitDirection == LimitDirection.Horizontal){
                type = relativeIndex < correctIndex ? WhichDirectionType.Left : WhichDirectionType.Right;
            }
            else{
                type = relativeIndex < correctIndex ? WhichDirectionType.Front : WhichDirectionType.Back;
            }
            builder.Replace("{2}", _uniqueNames[relativeIndex]);
            
            builder.Replace("{1}", _directionNames[type]);
            taskQuestion = builder.ToString(); 
        }
    }
}