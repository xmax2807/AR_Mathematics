using Project.Utils.ExtensionMethods;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Project.QuizSystem{
    public enum QuestionType {
        SingleChoice, ShortAnswer, Other
    }
    public enum QuestionContentType{
        Image,Text, Slider, None
    }
    public interface IQuestion {
        public QuestionType QuestionType {get;}
        public QuestionContentType QuestionContentType {get;}
        public string GetQuestion();
        public bool IsCorrect();
        public bool HasAnswered();
        public void SetAnswer(object value);
        public IQuestion Clone();
    }
    public abstract class BaseQuestion<T> : IQuestion{
        public abstract QuestionType QuestionType {get;}
        public abstract QuestionContentType QuestionContentType {get;}
        protected string _question;
        protected T _answer;
        protected T _playerAnswered;
        public T Answer => _playerAnswered;
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
            if(_answer is IEquatable<T> equatableAnswer){
                return equatableAnswer.Equals(_playerAnswered);
            }
            var comparer = GetEqualityComparer();
            if(comparer == null){
                UnityEngine.Debug.LogError("Can't compare 2 answer");
                return false;
            }
            return comparer.Equals(_answer, _playerAnswered);
        }
        protected virtual IEqualityComparer<T> GetEqualityComparer() => null;
        public bool IsCorrect(T answer){
            SetAnswer(answer);
            return IsCorrect();
        }

        public virtual void SetAnswer(object value)
        {
            value.TryCastTo<T>(onErrorFallback: OnSetAnswerFailed,out _playerAnswered);
        }

        private T OnSetAnswerFailed(object value, InvalidCastException e){
            UnityEngine.Debug.Log(e.Message);
            TrySetAnswer(value);
            return _answer;
        }
        protected virtual void TrySetAnswer(object value){}

        public abstract IQuestion Clone();
    }
}