using Project.Utils.ExtensionMethods;
using System;
using UnityEngine.UI;

namespace Project.QuizSystem{
    public enum QuestionType {
        SingleChoice, ShortAnswer, Other
    }
    public enum QuestionContentType{
        Image,Text, Slider
    }
    public interface IQuestion {
        public QuestionType QuestionType {get;}
        public QuestionContentType QuestionContentType {get;}
        public string GetQuestion();
        public bool IsCorrect();
        public bool HasAnswered();
        public void SetAnswer(object value);
    }
    public abstract class BaseQuestion<T> : IQuestion where T : IEquatable<T>{
        public abstract QuestionType QuestionType {get;}

        public abstract QuestionContentType QuestionContentType {get;}

        protected string _question;
        protected T _answer;
        private T _playerAnswered;
        public BaseQuestion(string question, T answer){
            _question = question;
            _answer = answer;
        }
        protected BaseQuestion(string question){
            _question = question;
        }

        public virtual bool HasAnswered() => !_playerAnswered.Equals(null);
        public virtual string GetQuestion()=>_question;
        // public virtual bool IsCorrect(T playerAns){
        //     return playerAns.Equals(_answer);
        // }
        public bool IsCorrect()
        {
            return _answer.Equals(_playerAnswered);
        }
        public bool IsCorrect(T answer){
            SetAnswer(answer);
            return IsCorrect();
        }

        public void SetAnswer(object value)
        {
            value.TryCastTo<T>(out _playerAnswered);
        }
    }
}