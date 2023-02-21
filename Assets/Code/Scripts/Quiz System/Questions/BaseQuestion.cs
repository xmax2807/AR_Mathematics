using Project.Utils.ExtensionMethods;
using System;
using UnityEngine.UI;

namespace Project.QuizSystem{
    // public enum QuestionType{
    //     SingleChoice, MultipleChoice
    // }
    public interface IQuestion {
        public bool IsCorrect();
        public bool HasAnswered();
        public void SetAnswer(object value);
        public void UpdateUI(LayoutGroup layout);
    }
    public abstract class BaseQuestion<T> : IQuestion where T : IEquatable<T>{
        private string _question;
        private T _answer;
        private T _playerAnswered;
        public BaseQuestion(string question, T answer){
            _question = question;
            _answer = answer;
        }

        public virtual bool HasAnswered() => !_playerAnswered.Equals(null);

        // public virtual bool IsCorrect(T playerAns){
        //     return playerAns.Equals(_answer);
        // }
        public bool IsCorrect()
        {
            return _answer.Equals(_playerAnswered);
        }

        public void SetAnswer(object value)
        {
            value.TryCastTo<T>(out _playerAnswered);
        }

        public void UpdateUI(LayoutGroup layout)
        {
            throw new NotImplementedException();
        }
    }
}